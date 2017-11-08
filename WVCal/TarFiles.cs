// Copyright 2017 The Johns Hopkins University Applied Physics Laboratory.
// Licensed under the MIT License. See LICENSE.txt in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


namespace WVCal {
	public static class TarFileExtraction {
		//extracts the first found .IMD file from an uncompressed tar archive
		// implementation supports only basic UStar
		public static string getIMDFromTar(string tarFilename) {
			int tarFileSize = (int)new FileInfo(tarFilename).Length;
			using (BinaryReader br = new BinaryReader(File.OpenRead(tarFilename), Encoding.ASCII)) {
				while (true) {
					//read header info for this block
					string hName = new string(br.ReadChars(100));
					string hMode = new string(br.ReadChars(8));
					string hUid = new string(br.ReadChars(8));
					string hGid = new string(br.ReadChars(8));
					string hSize = new string(br.ReadChars(12));
					string hMTime = new string(br.ReadChars(12));
					string hChecksum = new string(br.ReadChars(8));
					string hLink = new string(br.ReadChars(1));
					string hLinkname = new string(br.ReadChars(100));
					//extra UStar attributes
					string hMagic = new string(br.ReadChars(6));
					string hVersion = new string(br.ReadChars(2));
					string hUName = new string(br.ReadChars(32));
					string hGName = new string(br.ReadChars(32));
					string hDevMajor = new string(br.ReadChars(8));
					string hDevMinor = new string(br.ReadChars(8));
					string hPrefix = new string(br.ReadChars(155));
					br.ReadBytes(12);

					//calculate file size
					int fileByteSize = octStringToNumber(hSize);//file size
					int fileByteChunkSize = (int)Math.Ceiling(fileByteSize / 512.0) * 512;//file size rounded-up to nearest 512 byte chunk

					//return IMD file if found, otherwise skip to next file in archive
					string actualFilename = hName.Replace("\0", "");//strip extra characters
					string ext = Path.GetExtension(actualFilename);
					if (ext.ToLower().Equals(".imd")) {//IMD file found
						byte[] fileBytes = br.ReadBytes(fileByteSize);//read file data
						return System.Text.Encoding.UTF8.GetString(fileBytes);//return file as text
					} else {
						br.BaseStream.Seek(fileByteChunkSize, SeekOrigin.Current);//seek to next file position
					}

					if (br.BaseStream.Position >= tarFileSize) break;//stop reading at end of file
				}
			}
			return null;
		}

		//convert a base-8 character string to a regular int
		private static int octStringToNumber(string oct) {
			oct = oct.Replace("\0", "");
			oct = oct.TrimStart('0');

			int tot = 0;
			for (int i = 0; i < oct.Length; i++) {
				tot += (int)Math.Pow(8, i) * int.Parse(oct[oct.Length - 1 - i].ToString());
			}

			return tot;
		}

	}
}
