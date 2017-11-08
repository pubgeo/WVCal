namespace WVCal {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.ButtonGrid = new System.Windows.Forms.TableLayoutPanel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.ConvertButton = new System.Windows.Forms.Button();
			this.OutputGroupBox = new System.Windows.Forms.GroupBox();
			this.OutputGrid = new System.Windows.Forms.TableLayoutPanel();
			this.InputSkipConvertedCheck = new System.Windows.Forms.CheckBox();
			this.OutputScaleFactorLabel = new System.Windows.Forms.Label();
			this.OutputDataTypeLabel = new System.Windows.Forms.Label();
			this.OutputDataTypeCombo = new System.Windows.Forms.ComboBox();
			this.OutputScaleFactorText = new System.Windows.Forms.TextBox();
			this.InputGroupBox = new System.Windows.Forms.GroupBox();
			this.InputGrid = new System.Windows.Forms.TableLayoutPanel();
			this.InputTypeRadioFolder = new System.Windows.Forms.RadioButton();
			this.InputFolderText = new System.Windows.Forms.TextBox();
			this.InputFolderSelectButton = new System.Windows.Forms.Button();
			this.InputTypeRadioFile = new System.Windows.Forms.RadioButton();
			this.InputFileNITFLabel = new System.Windows.Forms.Label();
			this.InputFileNITFText = new System.Windows.Forms.TextBox();
			this.InputFileNITFSelectButton = new System.Windows.Forms.Button();
			this.InputFileIMDLabel = new System.Windows.Forms.Label();
			this.InputFileIMDText = new System.Windows.Forms.TextBox();
			this.InputFileIMDSelectButton = new System.Windows.Forms.Button();
			this.NotificationsText = new System.Windows.Forms.RichTextBox();
			this.NotificationsTextOld = new System.Windows.Forms.TextBox();
			this.StatusBarPanel = new System.Windows.Forms.Panel();
			this.StatusBarGrid = new System.Windows.Forms.TableLayoutPanel();
			this.StatusBarProgressGrid = new System.Windows.Forms.TableLayoutPanel();
			this.StatusBarOverallProgress = new System.Windows.Forms.ProgressBar();
			this.StatusBarFileProgress = new System.Windows.Forms.ProgressBar();
			this.StatusBarLabel = new System.Windows.Forms.Label();
			this.ToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.OutputApplyFineTuneCalibrationsCheck = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
			this.MainSplitContainer.Panel1.SuspendLayout();
			this.MainSplitContainer.Panel2.SuspendLayout();
			this.MainSplitContainer.SuspendLayout();
			this.ButtonGrid.SuspendLayout();
			this.OutputGroupBox.SuspendLayout();
			this.OutputGrid.SuspendLayout();
			this.InputGroupBox.SuspendLayout();
			this.InputGrid.SuspendLayout();
			this.StatusBarPanel.SuspendLayout();
			this.StatusBarGrid.SuspendLayout();
			this.StatusBarProgressGrid.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainSplitContainer
			// 
			this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.MainSplitContainer.Name = "MainSplitContainer";
			// 
			// MainSplitContainer.Panel1
			// 
			this.MainSplitContainer.Panel1.Controls.Add(this.ButtonGrid);
			this.MainSplitContainer.Panel1.Controls.Add(this.OutputGroupBox);
			this.MainSplitContainer.Panel1.Controls.Add(this.InputGroupBox);
			// 
			// MainSplitContainer.Panel2
			// 
			this.MainSplitContainer.Panel2.Controls.Add(this.NotificationsText);
			this.MainSplitContainer.Panel2.Controls.Add(this.NotificationsTextOld);
			this.MainSplitContainer.Size = new System.Drawing.Size(651, 593);
			this.MainSplitContainer.SplitterDistance = 394;
			this.MainSplitContainer.TabIndex = 4;
			this.MainSplitContainer.TabStop = false;
			this.MainSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.MainSplitContainer_SplitterMoved);
			// 
			// ButtonGrid
			// 
			this.ButtonGrid.AutoSize = true;
			this.ButtonGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonGrid.ColumnCount = 4;
			this.ButtonGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.ButtonGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.ButtonGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.ButtonGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.ButtonGrid.Controls.Add(this.CancelButton, 2, 0);
			this.ButtonGrid.Controls.Add(this.ConvertButton, 1, 0);
			this.ButtonGrid.Dock = System.Windows.Forms.DockStyle.Top;
			this.ButtonGrid.Location = new System.Drawing.Point(0, 261);
			this.ButtonGrid.Name = "ButtonGrid";
			this.ButtonGrid.RowCount = 1;
			this.ButtonGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.ButtonGrid.Size = new System.Drawing.Size(394, 36);
			this.ButtonGrid.TabIndex = 6;
			// 
			// CancelButton
			// 
			this.CancelButton.Location = new System.Drawing.Point(200, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(100, 30);
			this.CancelButton.TabIndex = 7;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// ConvertButton
			// 
			this.ConvertButton.Location = new System.Drawing.Point(94, 3);
			this.ConvertButton.Name = "ConvertButton";
			this.ConvertButton.Size = new System.Drawing.Size(100, 30);
			this.ConvertButton.TabIndex = 6;
			this.ConvertButton.Text = "Convert";
			this.ConvertButton.UseVisualStyleBackColor = true;
			this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
			// 
			// OutputGroupBox
			// 
			this.OutputGroupBox.AutoSize = true;
			this.OutputGroupBox.Controls.Add(this.OutputGrid);
			this.OutputGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.OutputGroupBox.Location = new System.Drawing.Point(0, 166);
			this.OutputGroupBox.Name = "OutputGroupBox";
			this.OutputGroupBox.Size = new System.Drawing.Size(394, 95);
			this.OutputGroupBox.TabIndex = 4;
			this.OutputGroupBox.TabStop = false;
			this.OutputGroupBox.Text = "Output";
			// 
			// OutputGrid
			// 
			this.OutputGrid.AutoSize = true;
			this.OutputGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.OutputGrid.ColumnCount = 2;
			this.OutputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.OutputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.OutputGrid.Controls.Add(this.OutputScaleFactorLabel, 0, 1);
			this.OutputGrid.Controls.Add(this.OutputDataTypeLabel, 0, 0);
			this.OutputGrid.Controls.Add(this.OutputDataTypeCombo, 1, 0);
			this.OutputGrid.Controls.Add(this.OutputScaleFactorText, 1, 1);
			this.OutputGrid.Controls.Add(this.OutputApplyFineTuneCalibrationsCheck, 0, 2);
			this.OutputGrid.Dock = System.Windows.Forms.DockStyle.Top;
			this.OutputGrid.Location = new System.Drawing.Point(3, 16);
			this.OutputGrid.Name = "OutputGrid";
			this.OutputGrid.RowCount = 3;
			this.OutputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OutputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OutputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OutputGrid.Size = new System.Drawing.Size(388, 76);
			this.OutputGrid.TabIndex = 7;
			// 
			// InputSkipConvertedCheck
			// 
			this.InputSkipConvertedCheck.AutoSize = true;
			this.InputSkipConvertedCheck.Checked = true;
			this.InputSkipConvertedCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.InputGrid.SetColumnSpan(this.InputSkipConvertedCheck, 3);
			this.InputSkipConvertedCheck.Location = new System.Drawing.Point(3, 127);
			this.InputSkipConvertedCheck.Name = "InputSkipConvertedCheck";
			this.InputSkipConvertedCheck.Size = new System.Drawing.Size(156, 17);
			this.InputSkipConvertedCheck.TabIndex = 8;
			this.InputSkipConvertedCheck.Text = "Skip already converted files";
			this.InputSkipConvertedCheck.UseVisualStyleBackColor = true;
			// 
			// OutputScaleFactorLabel
			// 
			this.OutputScaleFactorLabel.AutoSize = true;
			this.OutputScaleFactorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutputScaleFactorLabel.Location = new System.Drawing.Point(3, 27);
			this.OutputScaleFactorLabel.Name = "OutputScaleFactorLabel";
			this.OutputScaleFactorLabel.Size = new System.Drawing.Size(64, 26);
			this.OutputScaleFactorLabel.TabIndex = 3;
			this.OutputScaleFactorLabel.Text = "Scale factor";
			this.OutputScaleFactorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OutputDataTypeLabel
			// 
			this.OutputDataTypeLabel.AutoSize = true;
			this.OutputDataTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutputDataTypeLabel.Location = new System.Drawing.Point(3, 0);
			this.OutputDataTypeLabel.Name = "OutputDataTypeLabel";
			this.OutputDataTypeLabel.Size = new System.Drawing.Size(64, 27);
			this.OutputDataTypeLabel.TabIndex = 1;
			this.OutputDataTypeLabel.Text = "Data format";
			this.OutputDataTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OutputDataTypeCombo
			// 
			this.OutputDataTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.OutputDataTypeCombo.FormattingEnabled = true;
			this.OutputDataTypeCombo.Items.AddRange(new object[] {
            "UInt16",
            "Float32"});
			this.OutputDataTypeCombo.Location = new System.Drawing.Point(73, 3);
			this.OutputDataTypeCombo.Name = "OutputDataTypeCombo";
			this.OutputDataTypeCombo.Size = new System.Drawing.Size(94, 21);
			this.OutputDataTypeCombo.TabIndex = 4;
			// 
			// OutputScaleFactorText
			// 
			this.OutputScaleFactorText.Location = new System.Drawing.Point(73, 30);
			this.OutputScaleFactorText.Name = "OutputScaleFactorText";
			this.OutputScaleFactorText.Size = new System.Drawing.Size(94, 20);
			this.OutputScaleFactorText.TabIndex = 5;
			this.OutputScaleFactorText.TextChanged += new System.EventHandler(this.OutputScaleFactorText_TextChanged);
			// 
			// InputGroupBox
			// 
			this.InputGroupBox.AutoSize = true;
			this.InputGroupBox.Controls.Add(this.InputGrid);
			this.InputGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.InputGroupBox.Location = new System.Drawing.Point(0, 0);
			this.InputGroupBox.Name = "InputGroupBox";
			this.InputGroupBox.Size = new System.Drawing.Size(394, 166);
			this.InputGroupBox.TabIndex = 3;
			this.InputGroupBox.TabStop = false;
			this.InputGroupBox.Text = "Input";
			// 
			// InputGrid
			// 
			this.InputGrid.AutoSize = true;
			this.InputGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InputGrid.ColumnCount = 4;
			this.InputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.InputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.InputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.InputGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.InputGrid.Controls.Add(this.InputSkipConvertedCheck, 0, 5);
			this.InputGrid.Controls.Add(this.InputTypeRadioFolder, 0, 0);
			this.InputGrid.Controls.Add(this.InputFolderText, 2, 1);
			this.InputGrid.Controls.Add(this.InputFolderSelectButton, 3, 1);
			this.InputGrid.Controls.Add(this.InputTypeRadioFile, 0, 2);
			this.InputGrid.Controls.Add(this.InputFileNITFLabel, 1, 3);
			this.InputGrid.Controls.Add(this.InputFileNITFText, 2, 3);
			this.InputGrid.Controls.Add(this.InputFileNITFSelectButton, 3, 3);
			this.InputGrid.Controls.Add(this.InputFileIMDLabel, 1, 4);
			this.InputGrid.Controls.Add(this.InputFileIMDText, 2, 4);
			this.InputGrid.Controls.Add(this.InputFileIMDSelectButton, 3, 4);
			this.InputGrid.Dock = System.Windows.Forms.DockStyle.Top;
			this.InputGrid.Location = new System.Drawing.Point(3, 16);
			this.InputGrid.Name = "InputGrid";
			this.InputGrid.RowCount = 6;
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.InputGrid.Size = new System.Drawing.Size(388, 147);
			this.InputGrid.TabIndex = 7;
			// 
			// InputTypeRadioFolder
			// 
			this.InputTypeRadioFolder.AutoSize = true;
			this.InputGrid.SetColumnSpan(this.InputTypeRadioFolder, 2);
			this.InputTypeRadioFolder.Location = new System.Drawing.Point(3, 3);
			this.InputTypeRadioFolder.Name = "InputTypeRadioFolder";
			this.InputTypeRadioFolder.Size = new System.Drawing.Size(66, 17);
			this.InputTypeRadioFolder.TabIndex = 1;
			this.InputTypeRadioFolder.Text = "Folder    ";
			this.InputTypeRadioFolder.UseVisualStyleBackColor = true;
			this.InputTypeRadioFolder.CheckedChanged += new System.EventHandler(this.InputTypeRadioFolder_CheckedChanged);
			// 
			// InputFolderText
			// 
			this.InputFolderText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFolderText.Enabled = false;
			this.InputFolderText.Location = new System.Drawing.Point(75, 26);
			this.InputFolderText.Name = "InputFolderText";
			this.InputFolderText.Size = new System.Drawing.Size(276, 20);
			this.InputFolderText.TabIndex = 3;
			// 
			// InputFolderSelectButton
			// 
			this.InputFolderSelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFolderSelectButton.Enabled = false;
			this.InputFolderSelectButton.Location = new System.Drawing.Point(357, 26);
			this.InputFolderSelectButton.Name = "InputFolderSelectButton";
			this.InputFolderSelectButton.Size = new System.Drawing.Size(28, 20);
			this.InputFolderSelectButton.TabIndex = 4;
			this.InputFolderSelectButton.Text = "...";
			this.InputFolderSelectButton.UseVisualStyleBackColor = true;
			this.InputFolderSelectButton.Click += new System.EventHandler(this.InputFolderSelectButton_Click);
			// 
			// InputTypeRadioFile
			// 
			this.InputTypeRadioFile.AutoSize = true;
			this.InputTypeRadioFile.Checked = true;
			this.InputGrid.SetColumnSpan(this.InputTypeRadioFile, 2);
			this.InputTypeRadioFile.Location = new System.Drawing.Point(3, 52);
			this.InputTypeRadioFile.Name = "InputTypeRadioFile";
			this.InputTypeRadioFile.Size = new System.Drawing.Size(41, 17);
			this.InputTypeRadioFile.TabIndex = 5;
			this.InputTypeRadioFile.TabStop = true;
			this.InputTypeRadioFile.Text = "File";
			this.InputTypeRadioFile.UseVisualStyleBackColor = true;
			this.InputTypeRadioFile.CheckedChanged += new System.EventHandler(this.InputTypeRadioFile_CheckedChanged);
			// 
			// InputFileNITFLabel
			// 
			this.InputFileNITFLabel.AutoSize = true;
			this.InputFileNITFLabel.BackColor = System.Drawing.SystemColors.Control;
			this.InputFileNITFLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileNITFLabel.Location = new System.Drawing.Point(21, 72);
			this.InputFileNITFLabel.Name = "InputFileNITFLabel";
			this.InputFileNITFLabel.Size = new System.Drawing.Size(48, 26);
			this.InputFileNITFLabel.TabIndex = 7;
			this.InputFileNITFLabel.Text = "NITF";
			this.InputFileNITFLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InputFileNITFText
			// 
			this.InputFileNITFText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileNITFText.Location = new System.Drawing.Point(75, 75);
			this.InputFileNITFText.Name = "InputFileNITFText";
			this.InputFileNITFText.Size = new System.Drawing.Size(276, 20);
			this.InputFileNITFText.TabIndex = 8;
			// 
			// InputFileNITFSelectButton
			// 
			this.InputFileNITFSelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileNITFSelectButton.Location = new System.Drawing.Point(357, 75);
			this.InputFileNITFSelectButton.Name = "InputFileNITFSelectButton";
			this.InputFileNITFSelectButton.Size = new System.Drawing.Size(28, 20);
			this.InputFileNITFSelectButton.TabIndex = 9;
			this.InputFileNITFSelectButton.Text = "...";
			this.InputFileNITFSelectButton.UseVisualStyleBackColor = true;
			this.InputFileNITFSelectButton.Click += new System.EventHandler(this.InputFileNITFSelectButton_Click);
			// 
			// InputFileIMDLabel
			// 
			this.InputFileIMDLabel.AutoSize = true;
			this.InputFileIMDLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileIMDLabel.Location = new System.Drawing.Point(21, 98);
			this.InputFileIMDLabel.Name = "InputFileIMDLabel";
			this.InputFileIMDLabel.Size = new System.Drawing.Size(48, 26);
			this.InputFileIMDLabel.TabIndex = 10;
			this.InputFileIMDLabel.Text = "IMD/Tar";
			this.InputFileIMDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InputFileIMDText
			// 
			this.InputFileIMDText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileIMDText.Location = new System.Drawing.Point(75, 101);
			this.InputFileIMDText.Name = "InputFileIMDText";
			this.InputFileIMDText.Size = new System.Drawing.Size(276, 20);
			this.InputFileIMDText.TabIndex = 11;
			// 
			// InputFileIMDSelectButton
			// 
			this.InputFileIMDSelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InputFileIMDSelectButton.Location = new System.Drawing.Point(357, 101);
			this.InputFileIMDSelectButton.Name = "InputFileIMDSelectButton";
			this.InputFileIMDSelectButton.Size = new System.Drawing.Size(28, 20);
			this.InputFileIMDSelectButton.TabIndex = 12;
			this.InputFileIMDSelectButton.Text = "...";
			this.InputFileIMDSelectButton.UseVisualStyleBackColor = true;
			this.InputFileIMDSelectButton.Click += new System.EventHandler(this.InputFileIMDSelectButton_Click);
			// 
			// NotificationsText
			// 
			this.NotificationsText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NotificationsText.HideSelection = false;
			this.NotificationsText.Location = new System.Drawing.Point(0, 0);
			this.NotificationsText.Name = "NotificationsText";
			this.NotificationsText.ReadOnly = true;
			this.NotificationsText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.NotificationsText.Size = new System.Drawing.Size(253, 593);
			this.NotificationsText.TabIndex = 4;
			this.NotificationsText.Text = "";
			// 
			// NotificationsTextOld
			// 
			this.NotificationsTextOld.Location = new System.Drawing.Point(0, 0);
			this.NotificationsTextOld.Multiline = true;
			this.NotificationsTextOld.Name = "NotificationsTextOld";
			this.NotificationsTextOld.ReadOnly = true;
			this.NotificationsTextOld.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.NotificationsTextOld.Size = new System.Drawing.Size(253, 215);
			this.NotificationsTextOld.TabIndex = 3;
			this.NotificationsTextOld.TabStop = false;
			this.NotificationsTextOld.Text = "Test";
			// 
			// StatusBarPanel
			// 
			this.StatusBarPanel.AutoSize = true;
			this.StatusBarPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.StatusBarPanel.Controls.Add(this.StatusBarGrid);
			this.StatusBarPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.StatusBarPanel.Location = new System.Drawing.Point(0, 593);
			this.StatusBarPanel.Name = "StatusBarPanel";
			this.StatusBarPanel.Size = new System.Drawing.Size(651, 20);
			this.StatusBarPanel.TabIndex = 5;
			// 
			// StatusBarGrid
			// 
			this.StatusBarGrid.AutoSize = true;
			this.StatusBarGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.StatusBarGrid.ColumnCount = 2;
			this.StatusBarGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66F));
			this.StatusBarGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
			this.StatusBarGrid.Controls.Add(this.StatusBarProgressGrid, 0, 0);
			this.StatusBarGrid.Controls.Add(this.StatusBarLabel, 0, 0);
			this.StatusBarGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusBarGrid.Location = new System.Drawing.Point(0, 0);
			this.StatusBarGrid.Margin = new System.Windows.Forms.Padding(0);
			this.StatusBarGrid.Name = "StatusBarGrid";
			this.StatusBarGrid.RowCount = 1;
			this.StatusBarGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StatusBarGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.StatusBarGrid.Size = new System.Drawing.Size(651, 20);
			this.StatusBarGrid.TabIndex = 7;
			// 
			// StatusBarProgressGrid
			// 
			this.StatusBarProgressGrid.ColumnCount = 1;
			this.StatusBarProgressGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.StatusBarProgressGrid.Controls.Add(this.StatusBarOverallProgress, 0, 1);
			this.StatusBarProgressGrid.Controls.Add(this.StatusBarFileProgress, 0, 0);
			this.StatusBarProgressGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusBarProgressGrid.Location = new System.Drawing.Point(429, 0);
			this.StatusBarProgressGrid.Margin = new System.Windows.Forms.Padding(0);
			this.StatusBarProgressGrid.Name = "StatusBarProgressGrid";
			this.StatusBarProgressGrid.RowCount = 2;
			this.StatusBarProgressGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.StatusBarProgressGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.StatusBarProgressGrid.Size = new System.Drawing.Size(222, 20);
			this.StatusBarProgressGrid.TabIndex = 17;
			// 
			// StatusBarOverallProgress
			// 
			this.StatusBarOverallProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusBarOverallProgress.Location = new System.Drawing.Point(0, 10);
			this.StatusBarOverallProgress.Margin = new System.Windows.Forms.Padding(0);
			this.StatusBarOverallProgress.Name = "StatusBarOverallProgress";
			this.StatusBarOverallProgress.Size = new System.Drawing.Size(222, 10);
			this.StatusBarOverallProgress.TabIndex = 14;
			// 
			// StatusBarFileProgress
			// 
			this.StatusBarFileProgress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusBarFileProgress.Location = new System.Drawing.Point(0, 0);
			this.StatusBarFileProgress.Margin = new System.Windows.Forms.Padding(0);
			this.StatusBarFileProgress.Name = "StatusBarFileProgress";
			this.StatusBarFileProgress.Size = new System.Drawing.Size(222, 10);
			this.StatusBarFileProgress.TabIndex = 13;
			// 
			// StatusBarLabel
			// 
			this.StatusBarLabel.BackColor = System.Drawing.SystemColors.Control;
			this.StatusBarLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.StatusBarLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusBarLabel.Location = new System.Drawing.Point(0, 0);
			this.StatusBarLabel.Margin = new System.Windows.Forms.Padding(0);
			this.StatusBarLabel.Name = "StatusBarLabel";
			this.StatusBarLabel.Size = new System.Drawing.Size(429, 20);
			this.StatusBarLabel.TabIndex = 1;
			this.StatusBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OutputApplyFineTuneCalibrationsCheck
			// 
			this.OutputApplyFineTuneCalibrationsCheck.AutoSize = true;
			this.OutputApplyFineTuneCalibrationsCheck.Checked = true;
			this.OutputApplyFineTuneCalibrationsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.OutputGrid.SetColumnSpan(this.OutputApplyFineTuneCalibrationsCheck, 2);
			this.OutputApplyFineTuneCalibrationsCheck.Location = new System.Drawing.Point(3, 56);
			this.OutputApplyFineTuneCalibrationsCheck.Name = "OutputApplyFineTuneCalibrationsCheck";
			this.OutputApplyFineTuneCalibrationsCheck.Size = new System.Drawing.Size(160, 17);
			this.OutputApplyFineTuneCalibrationsCheck.TabIndex = 6;
			this.OutputApplyFineTuneCalibrationsCheck.Text = "Apply fine-tuning calibrations";
			this.OutputApplyFineTuneCalibrationsCheck.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(651, 613);
			this.Controls.Add(this.MainSplitContainer);
			this.Controls.Add(this.StatusBarPanel);
			this.Name = "MainForm";
			this.Text = "NITF image calibrator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MainSplitContainer.Panel1.ResumeLayout(false);
			this.MainSplitContainer.Panel1.PerformLayout();
			this.MainSplitContainer.Panel2.ResumeLayout(false);
			this.MainSplitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
			this.MainSplitContainer.ResumeLayout(false);
			this.ButtonGrid.ResumeLayout(false);
			this.OutputGroupBox.ResumeLayout(false);
			this.OutputGroupBox.PerformLayout();
			this.OutputGrid.ResumeLayout(false);
			this.OutputGrid.PerformLayout();
			this.InputGroupBox.ResumeLayout(false);
			this.InputGroupBox.PerformLayout();
			this.InputGrid.ResumeLayout(false);
			this.InputGrid.PerformLayout();
			this.StatusBarPanel.ResumeLayout(false);
			this.StatusBarPanel.PerformLayout();
			this.StatusBarGrid.ResumeLayout(false);
			this.StatusBarProgressGrid.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer MainSplitContainer;
		private System.Windows.Forms.GroupBox OutputGroupBox;
		private System.Windows.Forms.GroupBox InputGroupBox;
		private System.Windows.Forms.TextBox NotificationsTextOld;
		private System.Windows.Forms.Panel StatusBarPanel;
		private System.Windows.Forms.TableLayoutPanel InputGrid;
		private System.Windows.Forms.RadioButton InputTypeRadioFolder;
		private System.Windows.Forms.TextBox InputFolderText;
		private System.Windows.Forms.Button InputFolderSelectButton;
		private System.Windows.Forms.RadioButton InputTypeRadioFile;
		private System.Windows.Forms.Label InputFileNITFLabel;
		private System.Windows.Forms.TextBox InputFileNITFText;
		private System.Windows.Forms.Button InputFileNITFSelectButton;
		private System.Windows.Forms.Label InputFileIMDLabel;
		private System.Windows.Forms.TextBox InputFileIMDText;
		private System.Windows.Forms.Button InputFileIMDSelectButton;
		private System.Windows.Forms.TableLayoutPanel OutputGrid;
		private System.Windows.Forms.Label OutputScaleFactorLabel;
		private System.Windows.Forms.Label OutputDataTypeLabel;
		private System.Windows.Forms.ComboBox OutputDataTypeCombo;
		private System.Windows.Forms.TextBox OutputScaleFactorText;
		private System.Windows.Forms.TableLayoutPanel StatusBarGrid;
		private System.Windows.Forms.Label StatusBarLabel;
		private System.Windows.Forms.TableLayoutPanel ButtonGrid;
		private System.Windows.Forms.Button ConvertButton;
		private System.Windows.Forms.RichTextBox NotificationsText;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.TableLayoutPanel StatusBarProgressGrid;
		private System.Windows.Forms.ProgressBar StatusBarOverallProgress;
		private System.Windows.Forms.ProgressBar StatusBarFileProgress;
		private System.Windows.Forms.ToolTip ToolTips;
		private System.Windows.Forms.CheckBox InputSkipConvertedCheck;
		private System.Windows.Forms.CheckBox OutputApplyFineTuneCalibrationsCheck;

	}
}