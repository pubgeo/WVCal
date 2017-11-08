// Copyright 2017 The Johns Hopkins University Applied Physics Laboratory.
// Licensed under the MIT License. See LICENSE.txt in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OSGeo.GDAL;


namespace WVCal {
	public class Program {
		//win32 functions for handling the console window
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		const int SW_SHOW = 5;

		[STAThread]//required because we may be creating a GUI
		public static void Main(string[] args) {
			//initialize GDAL
			Gdal.AllRegister();
			//suppress warnings reported by GDAL (erroneously reports warnings because GDAL fails to create useless auxiliary files during processing)
			Gdal.SetErrorHandler(null);

			if (args.Length == 0) {//no arguments specified, so start GUI
				//hide console if we're using a GUI and the window belongs to this program (i.e. has an empty console)
				if (Console.CursorLeft == 0 && Console.CursorTop == 0) {
					IntPtr handle = GetConsoleWindow();
					ShowWindow(handle, SW_HIDE);
				}

				//run GUI
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());

				return;//exit after GUI closes
			} else {//arguments were specified, so run program as console command
				//parse input arguments into a convenient form (ToLookup allows duplicate keys)
				CommandArgField[] ops = CommandUtilities.parseArgs(args);
				Lookup<string, CommandArgField> lup = (Lookup<string, CommandArgField>)ops.ToLookup(p => p.argname);

				//display usage info and quit if -help specified
				if (lup.Contains("-help") == true) {
					printHelp();
					return;
				}

				//run file calibration
				runConvertCommand(lup);
			}
		}

		//display usage info in console
		private static void printHelp() {
			Console.WriteLine(@"DESCRIPTION
Calibrates WorldView NITF images to top-of-atmosphere radiance and outputs ENVI images.

SYNOPSIS
	WVCal -input file -output file [-imd file] [-outputdatatype type] [-scalefactor factor] [-applyfinetuningcalibration true/false]

OPTIONS
	-input file
		NITF file to be calibrated.
	-output file
		Name of ENVI file to be saved.
	-imd file
		IMD file to be used for image calibration.
	-outputdatatype type
		Supported types are 4 (float32) or 12 (uint16). Default is 12.
	-scalefactor factor
		All output values will be multiplied by factor before saving.
	-applyfinetuningcalibration
		Applies additional fine-tuning calibration constants.");
		}

		//parse inputs and attempt conversion of a file
		private static void runConvertCommand(Lookup<string, CommandArgField> lup) {
			//parse required input parameters
			string inputFilename = lup["-input"].ToArray()[0].parameters[0];
			string outputFilename = lup["-output"].ToArray()[0].parameters[0];

			//parse optional input parameters
			int outputdatatype = (lup.Contains("-outputdatatype") == true) ? int.Parse(lup["-outputdatatype"].ToArray()[0].parameters[0]) : 12;
			DataType outputType;
			if (outputdatatype == 4) {
				outputType = DataType.GDT_Float32;
			} else if (outputdatatype == 12) {
				outputType = DataType.GDT_UInt16;
			} else {
				throw new Exception("Unhandled output type. Only 4 (Float32) and 12 (UInt16) supported.");
			}

			double scaleFactor = (lup.Contains("-scalefactor") == true) ? double.Parse(lup["-scalefactor"].ToArray()[0].parameters[0]) : 1;

			string imdFilename;
			if (lup.Contains("-imd") == true) {
				imdFilename = lup["-imd"].ToArray()[0].parameters[0];
			} else {
				imdFilename = Path.GetDirectoryName(inputFilename) + "\\" + Path.GetFileNameWithoutExtension(inputFilename) + ".imd";
				if (File.Exists(imdFilename) == false) imdFilename = Path.GetDirectoryName(inputFilename) + "\\" + Path.GetFileNameWithoutExtension(inputFilename) + ".tar";
			}

			bool applyFineTuningCal = (lup.Contains("-applyfinetuningcalibration") == true) ? bool.Parse(lup["-applyfinetuningcalibration"].ToArray()[0].parameters[0]) : true;

			//setup background processing thread handler
			BackgroundWorker bw = new BackgroundWorker();
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;
			bw.DoWork += new DoWorkEventHandler(bw_DoWork);
			bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);

			//set parameters to pass to converter
			WVConversionSettings conversionSettings = new WVConversionSettings();
			conversionSettings.inputNITFFilename = inputFilename;
			conversionSettings.inputIMDTarFilename = imdFilename;
			conversionSettings.outputFilename = outputFilename;
			conversionSettings.outputType = outputType;
			conversionSettings.scaleFactor = scaleFactor;
			conversionSettings.applyFineTuningCal = applyFineTuningCal;
			conversionSettings.worker = bw;

			//start calibration in the background thread
			bw.RunWorkerAsync(conversionSettings);

			//wait for background processing to finish
			while (bw.IsBusy == true) {
				Thread.Sleep(100);
			}
		}

		//this function runs on the background thread
		private static void bw_DoWork(object sender, DoWorkEventArgs e) {
			try {
				CalibrateWV.CalibrateImage((WVConversionSettings)e.Argument);
				Console.WriteLine();
			} catch (Exception ex) {
			    //error occurred, so report it
			    Console.WriteLine("\nError! " + ex.Message);
			}
		}

		//report background conversion progress
		private static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			Console.Write("\r" + (string.Format("{0,3:###}", e.ProgressPercentage) + "% complete").PadRight(79));//print progress
		}
	}
}
