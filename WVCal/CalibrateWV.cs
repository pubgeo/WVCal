// Copyright 2017 The Johns Hopkins University Applied Physics Laboratory.
// Licensed under the MIT License. See LICENSE.txt in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using OSGeo.GDAL;


namespace WVCal {
	public class WVConversionSettings {
		public string inputNITFFilename;
		public string inputIMDTarFilename;
		public string outputFilename;
		public DataType outputType;
		public double scaleFactor;
		public bool applyFineTuningCal;
		public BackgroundWorker worker;
	}

	public static class CalibrateWV {
		public static void CalibrateImage(WVConversionSettings conversionSettings) {
			string inputFilename = conversionSettings.inputNITFFilename;
			string inputIMDFilename = conversionSettings.inputIMDTarFilename;
			string outputFilename = conversionSettings.outputFilename;
			string outputHeaderFilename = outputFilename + ".hdr";
			DataType outputType = conversionSettings.outputType;
			double scaleFactor = conversionSettings.scaleFactor;

			//open the input file
			Dataset ds = Gdal.Open(inputFilename, Access.GA_ReadOnly);
			if (ds == null) throw new Exception("Can't open " + inputFilename);//stop if file could not be opened properly

			//get GDAL driver for this file type
			Driver drv = ds.GetDriver();
			if (drv == null) throw new Exception("Can't get driver for reading.");//stop if driver could not be found for this file

			//get image raster info
			// GDAL reads image in blocks, so get their size & how many
			int width = ds.RasterXSize;
			int height = ds.RasterYSize;
			int nBands = ds.RasterCount;
			int blockXSize, blockYSize;
			ds.GetRasterBand(1).GetBlockSize(out blockXSize, out blockYSize);
			int nImageXBlocks = (int)Math.Ceiling((double)width / blockXSize);
			int nImageYBlocks = (int)Math.Ceiling((double)height / blockYSize);
			DataType inputDataType = ds.GetRasterBand(1).DataType;
			int bytesPerSample = getBytesInSampleType(inputDataType);

			//note on cache size
			// default cache size is ~41MB
			// read buffer must be less than cache size for improved speed
			// appears that gdal can read images using multi-threading with 1 core decoding each block.
			//   This works well as long as all currently needed blocks fit within the cache.
			// all bands for a block appear to be decoded at once
			// reading by block appears better because if we barely overrun the GDAL cache size the time doubles,
			//   whereas if reading by row all blocks intersecting the row must be read every time a row is read (far slower)

			//set GDAL cache to encompass one row of image blocks for all bands (plus a little extra)
			OSGeo.GDAL.Gdal.SetCacheMax(nImageXBlocks * blockXSize * blockYSize * nBands * bytesPerSample + 50000000);

			//parse band info from IMD file
			string IMDTarFilename = inputIMDFilename;
			string[] IMDLines;
			if (IMDTarFilename.Substring(IMDTarFilename.Length - 4, 4).ToLower().Equals(".imd")) {
				IMDLines = File.ReadAllLines(IMDTarFilename);
			} else {
				IMDLines = Regex.Split(TarFileExtraction.getIMDFromTar(IMDTarFilename), "\r\n|\r|\n");
			}
			BandInfo[] bandInfos = parseBandInfoFromIMD(IMDLines);

			//fine-tuning calibration info
			double[] fineTuneGains = new double[bandInfos.Length];
			double[] fineTuneOffsets = new double[bandInfos.Length];
			//set default values to make no changes
			for (int i = 0; i < bandInfos.Length; i++) fineTuneGains[i] = 1;
			//get fine-tune info if specified
			if (conversionSettings.applyFineTuningCal == true) getFineTuneFactors(bandInfos, fineTuneGains, fineTuneOffsets);

			//create new envi header file
			createENVIHeader(outputHeaderFilename, inputFilename, ds, bandInfos, outputType, scaleFactor);
			BinaryWriter bw = new BinaryWriter(new FileStream(outputFilename, FileMode.Create));

			//report start of work
			if (conversionSettings.worker != null) conversionSettings.worker.ReportProgress(0);

			//convert images one row of blocks at a time
			int bytesPerOutputSample = getBytesInSampleType(outputType);
			for (int ys = 0; ys < nImageYBlocks; ys++) {
				//allocate buffer to store one band of data for one block row
				int rowHeight = (ys < nImageYBlocks - 1) ? blockYSize : (height - ys * blockYSize);
				byte[] inputBuffer = new byte[width * rowHeight * bytesPerSample];
				double[][] dBuffer = new double[rowHeight][];
				byte[] outputBuffer = new byte[width * rowHeight * bytesPerOutputSample];
				
				for (int band = 1; band <= nBands; band++) {
					//read data
					System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(inputBuffer, System.Runtime.InteropServices.GCHandleType.Pinned);
					IntPtr pointer = handle.AddrOfPinnedObject();
					ds.GetRasterBand(band).ReadRaster(0, ys * blockYSize, width, rowHeight, pointer, width, rowHeight, ds.GetRasterBand(1).DataType, 0, 0);
					handle.Free();

					//convert raw input data to double precision
					for (int y = 0; y < rowHeight; y++) dBuffer[y] = new double[width];
					convertByteArrayToDouble2D(dBuffer, inputBuffer, inputDataType);

					//calibrate
					for (int y = 0; y < rowHeight; y++) {
						for (int x = 0; x < width; x++) {
							dBuffer[y][x] = (fineTuneGains[band - 1] * dBuffer[y][x] * (bandInfos[band - 1].absCalFactor / bandInfos[band - 1].effectiveBandwidth) + fineTuneOffsets[band - 1]) / 10 * scaleFactor;
						}
					}

					//convert calibrated info to output data type
					convertDouble2DToByteArray(dBuffer, outputBuffer, outputType);

					//save calibrated data to output file
					long bToIndex = ((long)(band - 1) * width * height + ys * blockYSize * width + 0) * bytesPerOutputSample;
					bw.Flush();
					bw.BaseStream.Seek(bToIndex, SeekOrigin.Begin);
					bw.Write(outputBuffer);

					//stop processing if work cancelled
					if (conversionSettings.worker != null && conversionSettings.worker.CancellationPending == true) {
						//stop writing to the file
						bw.Close();
						//try to delete incomplete output files
						File.Delete(outputFilename);
						File.Delete(outputHeaderFilename);

						//stop processing
						return;
					}

					//report progress
					if (conversionSettings.worker != null) conversionSettings.worker.ReportProgress((int)(((double)ys * nBands + band - 1 + 1) / (nImageYBlocks * nBands) * 100));
				}
			}

			//close output file
			bw.Close();
		}

		private static void convertByteArrayToDouble2D(double[][] imageData, byte[] inputBuffer, DataType inputType) {
			int width = imageData[0].Length;
			int height = imageData.Length;

			int bytesPerSample = getBytesInSampleType(inputType);
			Parallel.For(0, height, y => {
				int bufferIndex = bytesPerSample * width * y;

				for (int x = 0; x < width; x++) {
					if (inputType == DataType.GDT_Byte) {
						imageData[y][x] = inputBuffer[bufferIndex];
					} else if (inputType == DataType.GDT_UInt16) {
						imageData[y][x] = inputBuffer[bufferIndex + 1] * 256 + inputBuffer[bufferIndex];
					} else if (inputType == DataType.GDT_Float32) {
						imageData[y][x] = BitConverter.ToSingle(inputBuffer, bufferIndex);
					}

					//increment to next sample
					bufferIndex += bytesPerSample;
				}
			});
		}

		private static void convertDouble2DToByteArray(double[][] imageData, byte[] outputBuffer, DataType outputType) {
			int width = imageData[0].Length;
			int height = imageData.Length;

			int bytesPerSample = getBytesInSampleType(outputType);
			Parallel.For(0, height, y => {
				int outputBufferIndex = bytesPerSample * width * y;

				for (int x = 0; x < width; x++) {
					if (outputType == DataType.GDT_UInt16) {
						//clip value to 16-bit range (prevents overflow/underflow errors)
						double val = imageData[y][x];
						if (val > 65535) val = 65535;
						if (val < 0) val = 0;

						Array.Copy(BitConverter.GetBytes((UInt16)val), 0, outputBuffer, outputBufferIndex, 2);
					} else if (outputType == DataType.GDT_Float32) {
						Array.Copy(BitConverter.GetBytes((float)imageData[y][x]), 0, outputBuffer, outputBufferIndex, 4);
					}

					//increment to next sample
					outputBufferIndex += bytesPerSample;
				}
			});
		}

		private static int getBytesInSampleType(DataType type) {
			if (type == DataType.GDT_Byte) {
				return 1;
			} else if (type == DataType.GDT_UInt16) {
				return 2;
			} else if (type == DataType.GDT_Float32) {
				return 4;
			} else {
				throw new Exception("Data format not supported");
			}
		}

		//creates an ENVI header file with the same layout as ENVI outputs when calibrating a file
		public static void createENVIHeader(string headerFilename, string datasetFilename, Dataset ds, BandInfo[] bandInfos, DataType outputDataType, double scaleFactor) {
			TextWriter tw = new StreamWriter(headerFilename);

			tw.WriteLine("ENVI");
			tw.WriteLine("description = {WorldView Calibration Result, units = MicroWatts/(square centimeter * steradian * nanometer) [" + DateTime.Now.ToString("ddd MMM d yyyy HH:mm:ss") + "]}");
			tw.WriteLine("samples = " + ds.RasterXSize);
			tw.WriteLine("lines = " + ds.RasterYSize);
			tw.WriteLine("bands = " + ds.RasterCount);
			tw.WriteLine("header offset = 0");
			tw.WriteLine("file type = ENVI Standard");
			if (outputDataType == DataType.GDT_Byte) {
				tw.WriteLine("data type = 1");
			}else if (outputDataType == DataType.GDT_UInt16) {
				tw.WriteLine("data type = 12");
			} else if (outputDataType == DataType.GDT_Float32) {
				tw.WriteLine("data type = 4");
			}
			tw.WriteLine("interleave = bsq");
			tw.WriteLine("byte order = 0");

			//coordinate information can be in either of these two places, so just pick the longer one (the other is usually empty)
			string coordinateSystemString1 = ds.GetGCPProjection();
			string coordinateSystemString2 = ds.GetProjection();
			string coordinateSystemStringBest = (coordinateSystemString1.Length > coordinateSystemString2.Length) ? coordinateSystemString1 : coordinateSystemString2;
			tw.WriteLine("coordinate system string = {" + coordinateSystemStringBest + "}");
			
			//write default RGB bands if present
			int rIndex = -1, gIndex = -1, bIndex = -1;
			for (int b = 0; b < bandInfos.Length; b++) {
				if (bandInfos[b].bandCode.Equals("R")) rIndex = b;
				if (bandInfos[b].bandCode.Equals("G")) gIndex = b;
				if (bandInfos[b].bandCode.Equals("B")) bIndex = b;
			}
			if (rIndex > -1 && gIndex > -1 && bIndex > -1) tw.WriteLine("default bands = {" + (rIndex + 1) + "," + (gIndex + 1) + "," + (bIndex + 1) + "}");

			tw.WriteLine("wavelength units = Nanometers");

			tw.WriteLine("geo points = {");
			GCP[] gcps = ds.GetGCPs();
			if (gcps == null) {//GDAL couldn't read corner coordinates, so try to parse them ourselves (GDAL compatibility issue with WorldView3 NITFs?)
				string cscrna = ds.GetMetadataItem("CSCRNA", "TRE");
				if (cscrna != null) {
					gcps = new GCP[4];
					int cscidx = 1;
					for (int i = 0; i < 4; i++) {
						double lat = double.Parse(cscrna.Substring(cscidx, 9)); cscidx += 9;
						double lon = double.Parse(cscrna.Substring(cscidx, 10)); cscidx += 10;

						int pixelx, pixely;
						if (i == 0) {
							pixelx = 1; pixely = 1;
						} else if (i == 1) {
							pixelx = ds.RasterXSize + 1; pixely = 1;
						} else if (i == 2) {
							pixelx = ds.RasterXSize + 1; pixely = ds.RasterYSize + 1;
						} else {
							pixelx = 1; pixely = ds.RasterYSize + 1;
						}

						cscidx += 8;
						gcps[i] = new GCP(lon, lat, 0, pixelx, pixely, "", "");
					}
				}
			}
			if (gcps != null) {//write GCP info if available
				//swap 3rd and 4th GCPs, because NITF stores in UL,UR,LR,LL order, but envi wants UL,UR,LL,LR order
				GCP GCPtmp = gcps[2];
				gcps[2] = gcps[3];
				gcps[3] = GCPtmp;
				//write GCPs
				for (int i = 0; i < gcps.Length; i++) {
					tw.WriteLine(" " + gcps[i].GCPPixel + ", " + gcps[i].GCPLine + ", " + gcps[i].GCPY + ", " + gcps[i].GCPX + ((i == gcps.Length - 1) ? "}" : ""));
				}
			}

			tw.WriteLine("pseudo projection info = {Geographic Lat/Lon, WGS-84, units=Degrees}");

			tw.WriteLine("band names = {");
			for (int i = 0; i < bandInfos.Length; i++) {
				tw.WriteLine(" WV Calibrated Radiance: Band " + (i + 1) + " (" + bandInfos[i].bandCode + ")" + ((i == bandInfos.Length - 1) ? "}" : ", "));
			}

			//parse wavelengths and write to header
			double[] wavelengths = new double[bandInfos.Length];
			string cssfaa = ds.GetMetadataItem("CSSFAA", "TRE");
			if (cssfaa != null) {//parse wavelength data if available
				int iPos = 1;
				for (int i = 0; i < wavelengths.Length; i++) {
					iPos++;
					wavelengths[i] = double.Parse(cssfaa.Substring(iPos, 6)); iPos += 6;
					iPos += 99;
				}
				tw.WriteLine("wavelength = {");
				tw.Write(" ");
				for (int i = 0; i < wavelengths.Length; i++) tw.Write(wavelengths[i] + ((i == wavelengths.Length - 1) ? "}\n" : ", "));
			}

			//data gains
			tw.Write("data gain values = {\n ");
			for (int i = 0; i < bandInfos.Length; i++) tw.Write(bandInfos[i].gain + ((i == bandInfos.Length - 1) ? "}\n" : ", "));

			//data offset values
			tw.Write("data offset values = {\n ");
			for (int i = 0; i < bandInfos.Length; i++) tw.Write((i == bandInfos.Length - 1) ? "0}\n" : "0, ");

			//rpc coefficients
			List<double> rpcCoef = new List<double>();
			string rpc2 = ds.GetMetadataItem("RPC00B", "TRE");
			if (rpc2 != null) {//parse RPC data if available
				int charPos = 1 + 7 + 7;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 6))); charPos += 6;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 5))); charPos += 5;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 8))); charPos += 8;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 9))); charPos += 9;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 5))); charPos += 5;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 6))); charPos += 6;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 5))); charPos += 5;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 8))); charPos += 8;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 9))); charPos += 9;
				rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 5))); charPos += 5;
				for (int i = 0; i < 80; i++) { rpcCoef.Add(double.Parse(rpc2.Substring(charPos, 12))); charPos += 12; }
				rpcCoef.Add(0);
				rpcCoef.Add(0);
				rpcCoef.Add(1);

				tw.WriteLine("rpc info = {");
				tw.Write(" ");
				for (int i = 0; i < rpcCoef.Count; i++) {
					tw.Write(rpcCoef[i].ToString(" 0.00000000e+000;-0.00000000e+000"));

					if (i != rpcCoef.Count - 1) {
						if (i % 4 == 3) tw.Write(",\n "); else tw.Write(", ");
					} else {
						tw.Write("}\n");
					}
				}
			}

			//write scale factor for all values if specified
			if (scaleFactor != 1) tw.WriteLine("radiance_scale_factor = " + scaleFactor);

			tw.Close();
		}

		private static BandInfo[] parseBandInfoFromIMD(string[] IMDFileLines) {
			//parse satellite name
			string satelliteName = "";
			for (int l = 0; l < IMDFileLines.Length; l++) {
				if (IMDFileLines[l].Contains("satId")) {
					satelliteName = IMDFileLines[l].Split(new char[] { '"' })[1];
				}
			}

			//parse band calibration data
			List<BandInfo> bandInfos = new System.Collections.Generic.List<BandInfo>();
			for (int l = 0; l < IMDFileLines.Length; l++) {
				if (IMDFileLines[l].Contains("BEGIN_GROUP = BAND")) {
					BandInfo info = new BandInfo();

					info.bandCode = IMDFileLines[l].Substring(IMDFileLines[l].IndexOf("BAND_") + 5);

					l++;
					while (IMDFileLines[l].Contains("END_GROUP") == false) {
						string[] fields = IMDFileLines[l].TrimEnd(';').Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

						if (fields[0].Equals("ULLon")) info.ULLon = double.Parse(fields[2]);
						if (fields[0].Equals("ULLat")) info.ULLat = double.Parse(fields[2]);
						if (fields[0].Equals("URLon")) info.URLon = double.Parse(fields[2]);
						if (fields[0].Equals("URLat")) info.URLat = double.Parse(fields[2]);
						if (fields[0].Equals("LRLon")) info.LRLon = double.Parse(fields[2]);
						if (fields[0].Equals("LRLat")) info.LRLat = double.Parse(fields[2]);
						if (fields[0].Equals("LLLon")) info.LLLon = double.Parse(fields[2]);
						if (fields[0].Equals("LLLat")) info.LLLat = double.Parse(fields[2]);

						if (fields[0].Equals("absCalFactor")) info.absCalFactor = double.Parse(fields[2]);
						if (fields[0].Equals("effectiveBandwidth")) info.effectiveBandwidth = double.Parse(fields[2]);

						l++;
					}

					info.satelliteName = satelliteName;

					bandInfos.Add(info);
				}
			}

			return bandInfos.ToArray();
		}

		//loads fine-tuning calibration info for bands in the input image
		// only WV2,WV3 supported
		// values are from DigitalGlobe document ABSRADCAL_FLEET_2016v0_Rel20170403.pdf
		private static void getFineTuneFactors(BandInfo[] bandInfos, double[] fineGains, double[] fineOffsets) {
			//WorldView-3 constants
			Dictionary<string, double> WV3FineTuneGain = new Dictionary<string, double>{
				{"P", 0.950},
				{"C", 0.905},
				{"B", 0.940},
				{"G", 0.938},
				{"Y", 0.962},
				{"R", 0.964},
				{"RE", 1.000},
				{"N", 0.961},
				{"N2", 0.978},
				{"S1", 1.200},
				{"S2", 1.227},
				{"S3", 1.199},
				{"S4", 1.196},
				{"S5", 1.262},
				{"S6", 1.314},
				{"S7", 1.346},
				{"S8", 1.376}
			};
			Dictionary<string, double> WV3FineTuneOffset = new Dictionary<string, double>{
				{"P", -3.629},
				{"C", -8.604},
				{"B", -5.809},
				{"G", -4.996},
				{"Y", -3.649},
				{"R", -3.021},
				{"RE", -4.521},
				{"N", -5.522},
				{"N2", -2.992},
				{"S1", -5.546},
				{"S2", -2.600},
				{"S3", -2.309},
				{"S4", -1.676},
				{"S5", -0.705},
				{"S6", -0.669},
				{"S7", -0.512},
				{"S8", -0.372}
			};

			//WorldView-2 constants
			Dictionary<string, double> WV2FineTuneGain = new Dictionary<string, double>{
				{"P", 0.942},
				{"C", 1.151},
				{"B", 0.988},
				{"G", 0.936},
				{"Y", 0.949},
				{"R", 0.952},
				{"RE", 0.974},
				{"N", 0.961},
				{"N2", 1.002}
			};
			Dictionary<string, double> WV2FineTuneOffset = new Dictionary<string, double>{
				{"P", -2.704},
				{"C", -7.478},
				{"B", -5.736},
				{"G", -3.546},
				{"Y", -3.564},
				{"R", -2.512},
				{"RE", -4.120},
				{"N", -3.300},
				{"N2", -2.891}
			};

			if (bandInfos[0].satelliteName.Equals("WV03")) {//WorldView-3
				for (int i = 0; i < bandInfos.Length; i++) fineGains[i] = WV3FineTuneGain[bandInfos[i].bandCode];
				for (int i = 0; i < bandInfos.Length; i++) fineOffsets[i] = WV3FineTuneOffset[bandInfos[i].bandCode];
			} else if (bandInfos[0].satelliteName.Equals("WV02")) {//WorldView-2
				for (int i = 0; i < bandInfos.Length; i++) fineGains[i] = WV2FineTuneGain[bandInfos[i].bandCode];
				for (int i = 0; i < bandInfos.Length; i++) fineOffsets[i] = WV2FineTuneOffset[bandInfos[i].bandCode];
			}
		}

		public class BandInfo {
			public string bandCode;
			public string satelliteName;
			public double ULLon, ULLat;
			public double URLon, URLat;
			public double LRLon, LRLat;
			public double LLLon, LLLat;
			public double absCalFactor;
			public double effectiveBandwidth;
			public double gain;
		}

	}
}
