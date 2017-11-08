// Copyright 2017 The Johns Hopkins University Applied Physics Laboratory.
// Licensed under the MIT License. See LICENSE.txt in the project root for full license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using OSGeo.GDAL;


namespace WVCal {
	public partial class MainForm : Form {
		//input/output file names container
		private class FileToConvert {
			public string NITFFilename;
			public string IMDTarFilename;
			public string outputFilename;
		}

		//background processing thread handler
		private BackgroundWorker bw = new BackgroundWorker();

		//conversion parameters set at time of conversion start
		private bool processingMultipleFiles = false;
		private int conversionFilesConverted = 0;
		private bool conversionSkipAlreadyConvertedFiles = true;
		private DataType conversionOutputDataType;
		private double conversionOutputScaleFactor;
		private bool conversionApplyFineTuningCal;
		private FileToConvert[] filesToConvert = new FileToConvert[0];
		
		private bool cancelling = false;//true if in the process of cancelling file conversion
		private string conversionErrorMessage = null;


		//FORM EVENTS

		public MainForm() {
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			//setup background worker thread
			bw.DoWork += new DoWorkEventHandler(bw_DoWork);
			bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;

			//set control defaults
			OutputDataTypeCombo.SelectedIndex = 1;
			OutputScaleFactorText.Text = "1";

			//setup controls available when not processing
			setControlAvailabilityDuringProcessing(false);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (bw.IsBusy) {//if currently processing images, verify that program should close
				DialogResult result = MessageBox.Show("Conversion in progress. Close anyway?", "Confirm close", MessageBoxButtons.YesNo);
				if (result == DialogResult.No) e.Cancel = true;
			}
		}

		//FORM CONTROLS HANDLING

		private void MainSplitContainer_SplitterMoved(object sender, SplitterEventArgs e) {
			this.ActiveControl = null;//keep splitter from receiving focus (much better appearance)
		}

		private void OutputScaleFactorText_TextChanged(object sender, EventArgs e) {
			double value;
			bool isValidNumber = double.TryParse(OutputScaleFactorText.Text, out value);
			if (isValidNumber) {//text is convertable to a number
				OutputScaleFactorText.BackColor = SystemColors.Window;
			} else {//text is not a valid number, so highlight in red
				OutputScaleFactorText.BackColor = Color.FromArgb((Color.Red.R + SystemColors.Window.R) / 2, (Color.Red.G + SystemColors.Window.G) / 2, (Color.Red.B + SystemColors.Window.B) / 2);
			}
		}

		private void ConvertButton_Click(object sender, EventArgs e) {
			//validate output scale settings
			double scaleFactor;
			bool isValidScaleFactor = double.TryParse(OutputScaleFactorText.Text, out scaleFactor);
			if (isValidScaleFactor == false) {
				printNotification("Bad scale factor.", Color.Red);
				return;//do not proceed with conversion
			}

			//store conversion values (in case user changes them during conversion)
			conversionFilesConverted = 0;
			conversionSkipAlreadyConvertedFiles = InputSkipConvertedCheck.Checked;
			conversionOutputDataType = (OutputDataTypeCombo.SelectedIndex == 0) ? DataType.GDT_UInt16 : DataType.GDT_Float32;
			conversionOutputScaleFactor = scaleFactor;
			conversionApplyFineTuningCal = OutputApplyFineTuneCalibrationsCheck.Checked;

			if (InputTypeRadioFolder.Checked == true) {//convert whole folder of files
				processingMultipleFiles = true;
				setControlAvailabilityDuringProcessing(true);//enable/disable appropriate interface buttons

				//get a list of all NITF files in folder
				string[] inputFiles;
				try {
					string[] extensions = { ".ntf", ".nitf" };
					inputFiles = Directory.GetFiles(InputFolderText.Text, "*.*").Where(f => extensions.Contains(System.IO.Path.GetExtension(f).ToLower())).ToArray();
					printNotification(inputFiles.Length + " files found in folder.");
				} catch (Exception ex) {
					printNotification("Bad path. Aborting.", Color.Red);
					return;
				}

				//generate input/output file names for all files
				filesToConvert = new FileToConvert[inputFiles.Length];
				for (int i = 0; i < filesToConvert.Length; i++) {
					filesToConvert[i] = new FileToConvert();
					filesToConvert[i].NITFFilename = inputFiles[i];
					filesToConvert[i].IMDTarFilename = Path.GetDirectoryName(inputFiles[i]) + "\\" + Path.GetFileNameWithoutExtension(inputFiles[i]) + ".imd";//try IMD file first
					if (File.Exists(filesToConvert[i].IMDTarFilename) == false) filesToConvert[i].IMDTarFilename = filesToConvert[i].IMDTarFilename.Substring(0, filesToConvert[i].IMDTarFilename.Length - 4) + ".tar";//try tar if IMD not present
					filesToConvert[i].outputFilename = Path.GetDirectoryName(inputFiles[i]) + "\\" + Path.GetFileNameWithoutExtension(inputFiles[i]) + "_cal";
				}

				StatusBarLabel.Text = "Conversion started";

				//start conversion process
				startConvertingNextFile();
			} else {//convert a single file
				//generate input/output file names for specified file
				try {
					filesToConvert = new FileToConvert[1];
					filesToConvert[0] = new FileToConvert();
					filesToConvert[0].NITFFilename = InputFileNITFText.Text;
					filesToConvert[0].IMDTarFilename = InputFileIMDText.Text;
					filesToConvert[0].outputFilename = Path.GetDirectoryName(InputFileNITFText.Text) + "\\" + Path.GetFileNameWithoutExtension(InputFileNITFText.Text) + "_cal";
				} catch (Exception ex) {
					printNotification("Bad file. Aborting,", Color.Red);
					return;//do not proceed with conversion
				}

				processingMultipleFiles = false;
				setControlAvailabilityDuringProcessing(true);//enable/disable appropriate interface buttons

				StatusBarLabel.Text = "Conversion started";

				//start conversion process
				startConvertingNextFile();
			}
		}

		private void CancelButton_Click(object sender, EventArgs e) {
			cancelling = true;
			bw.CancelAsync();//request background processing to stop

			StatusBarLabel.Text = "Cancelling conversion...";
		}

		private void setControlAvailabilityDuringProcessing(bool processing) {
			//set available conversion buttons depending on if conversion is in progress
			if (processing == true) {
				ConvertButton.Enabled = false;
				CancelButton.Enabled = true;
			} else {
				ConvertButton.Enabled = true;
				CancelButton.Enabled = false;
			}
			
			//show appropriate progress bars for single or multiple file conversion
			if (processingMultipleFiles == true) {
				//show both progress bars
				StatusBarProgressGrid.RowStyles[0] = new RowStyle(SizeType.Percent, 50);
				StatusBarProgressGrid.RowStyles[1] = new RowStyle(SizeType.Percent, 50);
			} else {
				//show only file progress bar
				StatusBarProgressGrid.RowStyles[0] = new RowStyle(SizeType.Percent, 100);
				StatusBarProgressGrid.RowStyles[1] = new RowStyle(SizeType.Percent, 0);
			}
		}

		private void InputTypeRadioFolder_CheckedChanged(object sender, EventArgs e) {
			//enable/disable controls for input setting
			InputFolderText.Enabled = InputTypeRadioFolder.Checked;
			InputFolderSelectButton.Enabled = InputTypeRadioFolder.Checked;
			InputFileNITFText.Enabled = InputTypeRadioFile.Checked;
			InputFileNITFSelectButton.Enabled = InputTypeRadioFile.Checked;
			InputFileIMDText.Enabled = InputTypeRadioFile.Checked;
			InputFileIMDSelectButton.Enabled = InputTypeRadioFile.Checked;
		}

		private void InputTypeRadioFile_CheckedChanged(object sender, EventArgs e) {
			//enable/disable controls for input setting
			InputFolderText.Enabled = InputTypeRadioFolder.Checked;
			InputFolderSelectButton.Enabled = InputTypeRadioFolder.Checked;
			InputFileNITFText.Enabled = InputTypeRadioFile.Checked;
			InputFileNITFSelectButton.Enabled = InputTypeRadioFile.Checked;
			InputFileIMDText.Enabled = InputTypeRadioFile.Checked;
			InputFileIMDSelectButton.Enabled = InputTypeRadioFile.Checked;
		}

		//file/folder selection events

		private void InputFolderSelectButton_Click(object sender, EventArgs e) {
			//show folder selection dialog
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.SelectedPath = InputFolderText.Text;
			if (fbd.ShowDialog() == DialogResult.OK) {
				//update text field
				InputFolderText.Text = fbd.SelectedPath;

				//automatically choose folder processing option
				InputTypeRadioFolder.Checked = true;
			}
		}

		private void InputFileNITFSelectButton_Click(object sender, EventArgs e) {
			//show file selection dialog for NITF files
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "NITF Files (.ntf)|*.ntf|All Files (*.*)|*.*";
			if (ofd.ShowDialog() == DialogResult.OK) {
				//update text field
				InputFileNITFText.Text = ofd.FileName;

				//automatically guess at IMD/Tar file name
				string auxFilename = Path.GetDirectoryName(ofd.FileName) + "\\" + Path.GetFileNameWithoutExtension(ofd.FileName) + ".tar";//try tar file first
				if (File.Exists(auxFilename) == false) auxFilename = auxFilename.Substring(0, auxFilename.Length - 4) + ".IMD";//if no tar file default to IMD
				InputFileIMDText.Text = auxFilename;

				//automatically choose file processing option
				InputTypeRadioFile.Checked = true;
			}
		}

		private void InputFileIMDSelectButton_Click(object sender, EventArgs e) {
			//show file selection dialog for IMD/Tar files
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "IMD Files (.imd, .tar)|*.imd;*.tar|All Files (*.*)|*.*";
			if (ofd.ShowDialog() == DialogResult.OK) {
				//update text field
				InputFileIMDText.Text = ofd.FileName;

				//automatically choose file processing option
				InputTypeRadioFile.Checked = true;
			}
		}

		//NOTIFICATIONS HANDLING

		private void printNotification(string message) {
			printNotification(message, NotificationsText.ForeColor);
		}
		private void printNotification(string message, Color textColor) {
			NotificationsText.SelectionStart = NotificationsText.TextLength;
			NotificationsText.SelectionLength = 0;
			NotificationsText.SelectionColor = textColor;
			NotificationsText.AppendText(message + Environment.NewLine);
			NotificationsText.SelectionColor = NotificationsText.ForeColor;
		}

		//BACKGROUND THREAD HANDLING

		//starts background thread to work on conversion
		private void convertNITFToENVI(WVConversionSettings conversionSettings) {
			conversionSettings.worker = bw;
			bw.RunWorkerAsync(conversionSettings);
		}

		//this function runs on the background thread
		private void bw_DoWork(object sender, DoWorkEventArgs e) {
			try {
				CalibrateWV.CalibrateImage((WVConversionSettings)e.Argument);
			} catch (Exception ex) {
				conversionErrorMessage = "Error! " + ex.Message;//some error occurred during conversion, so store message to report
			}
		}

		//progress has been made on converting a file
		private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			//update interface progress bars
			StatusBarFileProgress.Value = e.ProgressPercentage;
			StatusBarOverallProgress.Value = (int)(((double)conversionFilesConverted + e.ProgressPercentage / 100.0) / filesToConvert.Length * 100);
		}

		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			bw_FileConversionComplete();
		}

		//conversion of a file has finished
		// called even if work is cancelled
		private void bw_FileConversionComplete() {
			//report any error that occurred during conversion
			if (conversionErrorMessage != null) {
				printNotification(conversionErrorMessage, Color.Red);
				conversionErrorMessage = null;//clear error
			}

			if (cancelling == false) {//move on to next file if available
				conversionFilesConverted++;

				StatusBarLabel.Text = "Conversion " + conversionFilesConverted + "/" + filesToConvert.Length + " complete";

				if (conversionFilesConverted == filesToConvert.Length) {//finished converting all files
					setControlAvailabilityDuringProcessing(false);//enable appropriate controls
				} else {//not finished converting all files, so start next file
					startConvertingNextFile();
				}
			} else {//in-progress conversion cancelled, so stop processing images
				setControlAvailabilityDuringProcessing(false);
				printNotification("Conversion cancelled.", Color.Red);
				StatusBarLabel.Text = "Conversion cancelled";
				cancelling = false;
			}
		}

		private void startConvertingNextFile() {
			//skip conversion if output file already exists
			if (File.Exists(filesToConvert[conversionFilesConverted].outputFilename) == true && conversionSkipAlreadyConvertedFiles == true) {
				string ENVIFileNameOnly = Path.GetFileName(filesToConvert[conversionFilesConverted].outputFilename);
				printNotification(ENVIFileNameOnly + " already exists. Skipping.");
				bw_FileConversionComplete();//can't work on this file, so mark as finished
				return;
			}

			//skip conversion if IMD file cannot be found
			string NITFFileNameOnly = Path.GetFileName(filesToConvert[conversionFilesConverted].NITFFilename);
			if (File.Exists(filesToConvert[conversionFilesConverted].IMDTarFilename) == false) {
				printNotification("No IMD/Tar file for " + NITFFileNameOnly + ". Skipping.", Color.Red);
				bw_FileConversionComplete();//can't work on this file, so mark as finished
				return;
			}

			//everything ok to start conversion of the next file
			printNotification("Converting " + NITFFileNameOnly);

			//set parameters to pass to converter
			WVConversionSettings conversionSettings = new WVConversionSettings();
			conversionSettings.inputNITFFilename = filesToConvert[conversionFilesConverted].NITFFilename;
			conversionSettings.inputIMDTarFilename = filesToConvert[conversionFilesConverted].IMDTarFilename;
			conversionSettings.outputFilename = filesToConvert[conversionFilesConverted].outputFilename;
			conversionSettings.outputType = conversionOutputDataType;
			conversionSettings.scaleFactor = conversionOutputScaleFactor;
			conversionSettings.applyFineTuningCal = conversionApplyFineTuningCal;

			//convert the file
			convertNITFToENVI(conversionSettings);
		}

	}
}
