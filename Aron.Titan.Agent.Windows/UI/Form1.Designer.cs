namespace Aron.Titan.Agent.Windows
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            imageList1 = new ImageList(components);
            materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            tabOverview = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel6 = new TableLayoutPanel();
            lbWinVersion = new MaterialSkin.Controls.MaterialTextBox2();
            lbVirtualization = new MaterialSkin.Controls.MaterialTextBox2();
            lbVm = new MaterialSkin.Controls.MaterialTextBox2();
            lbMultipassStatus = new MaterialSkin.Controls.MaterialTextBox2();
            lbCPU = new MaterialSkin.Controls.MaterialTextBox2();
            lbMem = new MaterialSkin.Controls.MaterialTextBox2();
            lbDisk = new MaterialSkin.Controls.MaterialTextBox2();
            tableLayoutPanel7 = new TableLayoutPanel();
            btnLog = new MaterialSkin.Controls.MaterialButton();
            btnGoToDashboard = new MaterialSkin.Controls.MaterialButton();
            btnInstallMultipass = new MaterialSkin.Controls.MaterialButton();
            btnInstall = new MaterialSkin.Controls.MaterialButton();
            btnInstallVM = new MaterialSkin.Controls.MaterialButton();
            tableLayoutPanel8 = new TableLayoutPanel();
            btnStart1 = new MaterialSkin.Controls.MaterialButton();
            btnStart = new MaterialSkin.Controls.MaterialButton();
            lbServiceStatus = new MaterialSkin.Controls.MaterialTextBox2();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnSaveKey = new MaterialSkin.Controls.MaterialButton();
            lbKey = new MaterialSkin.Controls.MaterialTextBox2();
            tableLayoutPanel4 = new TableLayoutPanel();
            lbDataDir = new MaterialSkin.Controls.MaterialTextBox2();
            btnSelectDir = new MaterialSkin.Controls.MaterialButton();
            folderBrowserDialog1 = new FolderBrowserDialog();
            timer1 = new System.Windows.Forms.Timer(components);
            materialTabControl1.SuspendLayout();
            tabOverview.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // materialTabControl1
            // 
            materialTabControl1.Controls.Add(tabOverview);
            materialTabControl1.Depth = 0;
            materialTabControl1.Dock = DockStyle.Fill;
            materialTabControl1.Location = new Point(3, 64);
            materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            materialTabControl1.Multiline = true;
            materialTabControl1.Name = "materialTabControl1";
            materialTabControl1.SelectedIndex = 0;
            materialTabControl1.Size = new Size(858, 793);
            materialTabControl1.TabIndex = 1;
            // 
            // tabOverview
            // 
            tabOverview.BackColor = Color.White;
            tabOverview.Controls.Add(tableLayoutPanel1);
            tabOverview.Location = new Point(4, 28);
            tabOverview.Name = "tabOverview";
            tabOverview.Padding = new Padding(3);
            tabOverview.Size = new Size(850, 761);
            tabOverview.TabIndex = 0;
            tabOverview.Text = "Overview";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            tableLayoutPanel1.Size = new Size(844, 755);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel7, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 188);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(844, 567);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(lbWinVersion, 0, 0);
            tableLayoutPanel6.Controls.Add(lbVirtualization, 0, 1);
            tableLayoutPanel6.Controls.Add(lbVm, 0, 2);
            tableLayoutPanel6.Controls.Add(lbMultipassStatus, 0, 3);
            tableLayoutPanel6.Controls.Add(lbCPU, 0, 4);
            tableLayoutPanel6.Controls.Add(lbMem, 0, 5);
            tableLayoutPanel6.Controls.Add(lbDisk, 0, 6);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 7;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel6.Size = new Size(556, 561);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // lbWinVersion
            // 
            lbWinVersion.AnimateReadOnly = false;
            lbWinVersion.AutoCompleteMode = AutoCompleteMode.None;
            lbWinVersion.AutoCompleteSource = AutoCompleteSource.None;
            lbWinVersion.BackgroundImageLayout = ImageLayout.None;
            lbWinVersion.CharacterCasing = CharacterCasing.Normal;
            lbWinVersion.Depth = 0;
            lbWinVersion.Dock = DockStyle.Fill;
            lbWinVersion.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbWinVersion.HideSelection = true;
            lbWinVersion.Hint = "Windows 版本";
            lbWinVersion.LeadingIcon = null;
            lbWinVersion.Location = new Point(6, 6);
            lbWinVersion.Margin = new Padding(6);
            lbWinVersion.MaxLength = 32767;
            lbWinVersion.MouseState = MaterialSkin.MouseState.OUT;
            lbWinVersion.Name = "lbWinVersion";
            lbWinVersion.PasswordChar = '\0';
            lbWinVersion.PrefixSuffixText = null;
            lbWinVersion.ReadOnly = true;
            lbWinVersion.RightToLeft = RightToLeft.No;
            lbWinVersion.SelectedText = "";
            lbWinVersion.SelectionLength = 0;
            lbWinVersion.SelectionStart = 0;
            lbWinVersion.ShortcutsEnabled = true;
            lbWinVersion.Size = new Size(544, 48);
            lbWinVersion.TabIndex = 2;
            lbWinVersion.TabStop = false;
            lbWinVersion.TextAlign = HorizontalAlignment.Left;
            lbWinVersion.TrailingIcon = null;
            lbWinVersion.UseSystemPasswordChar = false;
            // 
            // lbVirtualization
            // 
            lbVirtualization.AnimateReadOnly = false;
            lbVirtualization.AutoCompleteMode = AutoCompleteMode.None;
            lbVirtualization.AutoCompleteSource = AutoCompleteSource.None;
            lbVirtualization.BackgroundImageLayout = ImageLayout.None;
            lbVirtualization.CharacterCasing = CharacterCasing.Normal;
            lbVirtualization.Depth = 0;
            lbVirtualization.Dock = DockStyle.Fill;
            lbVirtualization.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbVirtualization.HideSelection = true;
            lbVirtualization.Hint = "虛擬化狀態";
            lbVirtualization.LeadingIcon = null;
            lbVirtualization.Location = new Point(6, 86);
            lbVirtualization.Margin = new Padding(6);
            lbVirtualization.MaxLength = 32767;
            lbVirtualization.MouseState = MaterialSkin.MouseState.OUT;
            lbVirtualization.Name = "lbVirtualization";
            lbVirtualization.PasswordChar = '\0';
            lbVirtualization.PrefixSuffixText = null;
            lbVirtualization.ReadOnly = true;
            lbVirtualization.RightToLeft = RightToLeft.No;
            lbVirtualization.SelectedText = "";
            lbVirtualization.SelectionLength = 0;
            lbVirtualization.SelectionStart = 0;
            lbVirtualization.ShortcutsEnabled = true;
            lbVirtualization.Size = new Size(544, 48);
            lbVirtualization.TabIndex = 3;
            lbVirtualization.TabStop = false;
            lbVirtualization.TextAlign = HorizontalAlignment.Left;
            lbVirtualization.TrailingIcon = null;
            lbVirtualization.UseSystemPasswordChar = false;
            // 
            // lbVm
            // 
            lbVm.AnimateReadOnly = false;
            lbVm.AutoCompleteMode = AutoCompleteMode.None;
            lbVm.AutoCompleteSource = AutoCompleteSource.None;
            lbVm.BackgroundImageLayout = ImageLayout.None;
            lbVm.CharacterCasing = CharacterCasing.Normal;
            lbVm.Depth = 0;
            lbVm.Dock = DockStyle.Fill;
            lbVm.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbVm.HideSelection = true;
            lbVm.Hint = "VM 狀態";
            lbVm.LeadingIcon = null;
            lbVm.Location = new Point(6, 166);
            lbVm.Margin = new Padding(6);
            lbVm.MaxLength = 32767;
            lbVm.MouseState = MaterialSkin.MouseState.OUT;
            lbVm.Name = "lbVm";
            lbVm.PasswordChar = '\0';
            lbVm.PrefixSuffixText = null;
            lbVm.ReadOnly = true;
            lbVm.RightToLeft = RightToLeft.No;
            lbVm.SelectedText = "";
            lbVm.SelectionLength = 0;
            lbVm.SelectionStart = 0;
            lbVm.ShortcutsEnabled = true;
            lbVm.Size = new Size(544, 48);
            lbVm.TabIndex = 4;
            lbVm.TabStop = false;
            lbVm.TextAlign = HorizontalAlignment.Left;
            lbVm.TrailingIcon = null;
            lbVm.UseSystemPasswordChar = false;
            // 
            // lbMultipassStatus
            // 
            lbMultipassStatus.AnimateReadOnly = false;
            lbMultipassStatus.AutoCompleteMode = AutoCompleteMode.None;
            lbMultipassStatus.AutoCompleteSource = AutoCompleteSource.None;
            lbMultipassStatus.BackgroundImageLayout = ImageLayout.None;
            lbMultipassStatus.CharacterCasing = CharacterCasing.Normal;
            lbMultipassStatus.Depth = 0;
            lbMultipassStatus.Dock = DockStyle.Fill;
            lbMultipassStatus.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbMultipassStatus.HideSelection = true;
            lbMultipassStatus.Hint = "Multipass 狀態";
            lbMultipassStatus.LeadingIcon = null;
            lbMultipassStatus.Location = new Point(6, 246);
            lbMultipassStatus.Margin = new Padding(6);
            lbMultipassStatus.MaxLength = 32767;
            lbMultipassStatus.MouseState = MaterialSkin.MouseState.OUT;
            lbMultipassStatus.Name = "lbMultipassStatus";
            lbMultipassStatus.PasswordChar = '\0';
            lbMultipassStatus.PrefixSuffixText = null;
            lbMultipassStatus.ReadOnly = true;
            lbMultipassStatus.RightToLeft = RightToLeft.No;
            lbMultipassStatus.SelectedText = "";
            lbMultipassStatus.SelectionLength = 0;
            lbMultipassStatus.SelectionStart = 0;
            lbMultipassStatus.ShortcutsEnabled = true;
            lbMultipassStatus.Size = new Size(544, 48);
            lbMultipassStatus.TabIndex = 8;
            lbMultipassStatus.TabStop = false;
            lbMultipassStatus.TextAlign = HorizontalAlignment.Left;
            lbMultipassStatus.TrailingIcon = null;
            lbMultipassStatus.UseSystemPasswordChar = false;
            // 
            // lbCPU
            // 
            lbCPU.AnimateReadOnly = false;
            lbCPU.AutoCompleteMode = AutoCompleteMode.None;
            lbCPU.AutoCompleteSource = AutoCompleteSource.None;
            lbCPU.BackgroundImageLayout = ImageLayout.None;
            lbCPU.CharacterCasing = CharacterCasing.Normal;
            lbCPU.Depth = 0;
            lbCPU.Dock = DockStyle.Fill;
            lbCPU.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbCPU.HideSelection = true;
            lbCPU.Hint = "vCPUs";
            lbCPU.LeadingIcon = null;
            lbCPU.Location = new Point(6, 326);
            lbCPU.Margin = new Padding(6);
            lbCPU.MaxLength = 32767;
            lbCPU.MouseState = MaterialSkin.MouseState.OUT;
            lbCPU.Name = "lbCPU";
            lbCPU.PasswordChar = '\0';
            lbCPU.PrefixSuffixText = null;
            lbCPU.ReadOnly = true;
            lbCPU.RightToLeft = RightToLeft.No;
            lbCPU.SelectedText = "";
            lbCPU.SelectionLength = 0;
            lbCPU.SelectionStart = 0;
            lbCPU.ShortcutsEnabled = true;
            lbCPU.Size = new Size(544, 48);
            lbCPU.TabIndex = 5;
            lbCPU.TabStop = false;
            lbCPU.TextAlign = HorizontalAlignment.Left;
            lbCPU.TrailingIcon = null;
            lbCPU.UseSystemPasswordChar = false;
            // 
            // lbMem
            // 
            lbMem.AnimateReadOnly = false;
            lbMem.AutoCompleteMode = AutoCompleteMode.None;
            lbMem.AutoCompleteSource = AutoCompleteSource.None;
            lbMem.BackgroundImageLayout = ImageLayout.None;
            lbMem.CharacterCasing = CharacterCasing.Normal;
            lbMem.Depth = 0;
            lbMem.Dock = DockStyle.Fill;
            lbMem.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbMem.HideSelection = true;
            lbMem.Hint = "Memory (GB)";
            lbMem.LeadingIcon = null;
            lbMem.Location = new Point(6, 406);
            lbMem.Margin = new Padding(6);
            lbMem.MaxLength = 32767;
            lbMem.MouseState = MaterialSkin.MouseState.OUT;
            lbMem.Name = "lbMem";
            lbMem.PasswordChar = '\0';
            lbMem.PrefixSuffixText = null;
            lbMem.ReadOnly = true;
            lbMem.RightToLeft = RightToLeft.No;
            lbMem.SelectedText = "";
            lbMem.SelectionLength = 0;
            lbMem.SelectionStart = 0;
            lbMem.ShortcutsEnabled = true;
            lbMem.Size = new Size(544, 48);
            lbMem.TabIndex = 6;
            lbMem.TabStop = false;
            lbMem.TextAlign = HorizontalAlignment.Left;
            lbMem.TrailingIcon = null;
            lbMem.UseSystemPasswordChar = false;
            // 
            // lbDisk
            // 
            lbDisk.AnimateReadOnly = false;
            lbDisk.AutoCompleteMode = AutoCompleteMode.None;
            lbDisk.AutoCompleteSource = AutoCompleteSource.None;
            lbDisk.BackgroundImageLayout = ImageLayout.None;
            lbDisk.CharacterCasing = CharacterCasing.Normal;
            lbDisk.Depth = 0;
            lbDisk.Dock = DockStyle.Fill;
            lbDisk.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbDisk.HideSelection = true;
            lbDisk.Hint = "Disk (GB)";
            lbDisk.LeadingIcon = null;
            lbDisk.Location = new Point(6, 486);
            lbDisk.Margin = new Padding(6);
            lbDisk.MaxLength = 32767;
            lbDisk.MouseState = MaterialSkin.MouseState.OUT;
            lbDisk.Name = "lbDisk";
            lbDisk.PasswordChar = '\0';
            lbDisk.PrefixSuffixText = null;
            lbDisk.ReadOnly = true;
            lbDisk.RightToLeft = RightToLeft.No;
            lbDisk.SelectedText = "";
            lbDisk.SelectionLength = 0;
            lbDisk.SelectionStart = 0;
            lbDisk.ShortcutsEnabled = true;
            lbDisk.Size = new Size(544, 48);
            lbDisk.TabIndex = 7;
            lbDisk.TabStop = false;
            lbDisk.TextAlign = HorizontalAlignment.Left;
            lbDisk.TrailingIcon = null;
            lbDisk.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(btnLog, 0, 5);
            tableLayoutPanel7.Controls.Add(btnGoToDashboard, 0, 4);
            tableLayoutPanel7.Controls.Add(btnInstallMultipass, 0, 2);
            tableLayoutPanel7.Controls.Add(btnInstall, 0, 3);
            tableLayoutPanel7.Controls.Add(btnInstallVM, 0, 1);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(562, 0);
            tableLayoutPanel7.Margin = new Padding(0);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 6;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 30.4999275F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 13.9000149F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 13.9000187F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 13.900013F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 13.9000149F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 13.9000149F));
            tableLayoutPanel7.Size = new Size(282, 567);
            tableLayoutPanel7.TabIndex = 1;
            // 
            // btnLog
            // 
            btnLog.AutoSize = false;
            btnLog.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnLog.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnLog.Depth = 0;
            btnLog.Dock = DockStyle.Fill;
            btnLog.HighEmphasis = true;
            btnLog.Icon = null;
            btnLog.Location = new Point(4, 490);
            btnLog.Margin = new Padding(4, 6, 4, 6);
            btnLog.MouseState = MaterialSkin.MouseState.HOVER;
            btnLog.Name = "btnLog";
            btnLog.NoAccentTextColor = Color.Empty;
            btnLog.Size = new Size(274, 71);
            btnLog.TabIndex = 9;
            btnLog.Text = "查看 Log";
            btnLog.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnLog.UseAccentColor = false;
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // btnGoToDashboard
            // 
            btnGoToDashboard.AutoSize = false;
            btnGoToDashboard.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnGoToDashboard.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnGoToDashboard.Depth = 0;
            btnGoToDashboard.Dock = DockStyle.Fill;
            btnGoToDashboard.HighEmphasis = true;
            btnGoToDashboard.Icon = null;
            btnGoToDashboard.Location = new Point(4, 412);
            btnGoToDashboard.Margin = new Padding(4, 6, 4, 6);
            btnGoToDashboard.MouseState = MaterialSkin.MouseState.HOVER;
            btnGoToDashboard.Name = "btnGoToDashboard";
            btnGoToDashboard.NoAccentTextColor = Color.Empty;
            btnGoToDashboard.Size = new Size(274, 66);
            btnGoToDashboard.TabIndex = 8;
            btnGoToDashboard.Text = "前往後臺";
            btnGoToDashboard.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnGoToDashboard.UseAccentColor = false;
            btnGoToDashboard.UseVisualStyleBackColor = true;
            btnGoToDashboard.Click += btnGoToDashboard_Click;
            // 
            // btnInstallMultipass
            // 
            btnInstallMultipass.AutoSize = false;
            btnInstallMultipass.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnInstallMultipass.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnInstallMultipass.Depth = 0;
            btnInstallMultipass.Dock = DockStyle.Fill;
            btnInstallMultipass.HighEmphasis = true;
            btnInstallMultipass.Icon = null;
            btnInstallMultipass.Location = new Point(4, 256);
            btnInstallMultipass.Margin = new Padding(4, 6, 4, 6);
            btnInstallMultipass.MouseState = MaterialSkin.MouseState.HOVER;
            btnInstallMultipass.Name = "btnInstallMultipass";
            btnInstallMultipass.NoAccentTextColor = Color.Empty;
            btnInstallMultipass.Size = new Size(274, 66);
            btnInstallMultipass.TabIndex = 7;
            btnInstallMultipass.Text = "安裝Multipass";
            btnInstallMultipass.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnInstallMultipass.UseAccentColor = false;
            btnInstallMultipass.UseVisualStyleBackColor = true;
            btnInstallMultipass.Click += btnInstallMultipass_Click;
            // 
            // btnInstall
            // 
            btnInstall.AutoSize = false;
            btnInstall.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnInstall.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnInstall.Depth = 0;
            btnInstall.Dock = DockStyle.Fill;
            btnInstall.HighEmphasis = true;
            btnInstall.Icon = null;
            btnInstall.Location = new Point(4, 334);
            btnInstall.Margin = new Padding(4, 6, 4, 6);
            btnInstall.MouseState = MaterialSkin.MouseState.HOVER;
            btnInstall.Name = "btnInstall";
            btnInstall.NoAccentTextColor = Color.Empty;
            btnInstall.Size = new Size(274, 66);
            btnInstall.TabIndex = 5;
            btnInstall.Text = "安裝服務";
            btnInstall.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnInstall.UseAccentColor = false;
            btnInstall.UseVisualStyleBackColor = true;
            btnInstall.Click += btnInstall_Click;
            // 
            // btnInstallVM
            // 
            btnInstallVM.AutoSize = false;
            btnInstallVM.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnInstallVM.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnInstallVM.Depth = 0;
            btnInstallVM.Dock = DockStyle.Fill;
            btnInstallVM.HighEmphasis = true;
            btnInstallVM.Icon = null;
            btnInstallVM.Location = new Point(4, 178);
            btnInstallVM.Margin = new Padding(4, 6, 4, 6);
            btnInstallVM.MouseState = MaterialSkin.MouseState.HOVER;
            btnInstallVM.Name = "btnInstallVM";
            btnInstallVM.NoAccentTextColor = Color.Empty;
            btnInstallVM.Size = new Size(274, 66);
            btnInstallVM.TabIndex = 2;
            btnInstallVM.Text = "安裝VM";
            btnInstallVM.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnInstallVM.UseAccentColor = false;
            btnInstallVM.UseVisualStyleBackColor = true;
            btnInstallVM.Click += btnInstallVM_Click;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 1;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Controls.Add(btnStart1, 0, 2);
            tableLayoutPanel8.Controls.Add(btnStart, 0, 1);
            tableLayoutPanel8.Controls.Add(lbServiceStatus, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 3);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 3;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.Size = new Size(276, 166);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // btnStart1
            // 
            btnStart1.AutoSize = false;
            btnStart1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnStart1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnStart1.Depth = 0;
            btnStart1.Dock = DockStyle.Fill;
            btnStart1.HighEmphasis = true;
            btnStart1.Icon = null;
            btnStart1.Location = new Point(4, 116);
            btnStart1.Margin = new Padding(4, 6, 4, 6);
            btnStart1.MouseState = MaterialSkin.MouseState.HOVER;
            btnStart1.Name = "btnStart1";
            btnStart1.NoAccentTextColor = Color.Empty;
            btnStart1.Size = new Size(268, 44);
            btnStart1.TabIndex = 7;
            btnStart1.Text = "直接啟動";
            btnStart1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnStart1.UseAccentColor = false;
            btnStart1.UseVisualStyleBackColor = true;
            btnStart1.Click += btnStart1_Click;
            // 
            // btnStart
            // 
            btnStart.AutoSize = false;
            btnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnStart.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnStart.Depth = 0;
            btnStart.Dock = DockStyle.Fill;
            btnStart.HighEmphasis = true;
            btnStart.Icon = null;
            btnStart.Location = new Point(4, 61);
            btnStart.Margin = new Padding(4, 6, 4, 6);
            btnStart.MouseState = MaterialSkin.MouseState.HOVER;
            btnStart.Name = "btnStart";
            btnStart.NoAccentTextColor = Color.Empty;
            btnStart.Size = new Size(268, 43);
            btnStart.TabIndex = 6;
            btnStart.Text = "啟動服務";
            btnStart.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnStart.UseAccentColor = false;
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // lbServiceStatus
            // 
            lbServiceStatus.AnimateReadOnly = false;
            lbServiceStatus.AutoCompleteMode = AutoCompleteMode.None;
            lbServiceStatus.AutoCompleteSource = AutoCompleteSource.None;
            lbServiceStatus.BackgroundImageLayout = ImageLayout.None;
            lbServiceStatus.CharacterCasing = CharacterCasing.Normal;
            lbServiceStatus.Depth = 0;
            lbServiceStatus.Dock = DockStyle.Fill;
            lbServiceStatus.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbServiceStatus.HideSelection = true;
            lbServiceStatus.Hint = "服務狀態";
            lbServiceStatus.LeadingIcon = null;
            lbServiceStatus.Location = new Point(6, 6);
            lbServiceStatus.Margin = new Padding(6);
            lbServiceStatus.MaxLength = 32767;
            lbServiceStatus.MouseState = MaterialSkin.MouseState.OUT;
            lbServiceStatus.Name = "lbServiceStatus";
            lbServiceStatus.PasswordChar = '\0';
            lbServiceStatus.PrefixSuffixText = null;
            lbServiceStatus.ReadOnly = true;
            lbServiceStatus.RightToLeft = RightToLeft.No;
            lbServiceStatus.SelectedText = "";
            lbServiceStatus.SelectionLength = 0;
            lbServiceStatus.SelectionStart = 0;
            lbServiceStatus.ShortcutsEnabled = true;
            lbServiceStatus.Size = new Size(264, 48);
            lbServiceStatus.TabIndex = 3;
            lbServiceStatus.TabStop = false;
            lbServiceStatus.TextAlign = HorizontalAlignment.Left;
            lbServiceStatus.TrailingIcon = null;
            lbServiceStatus.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(844, 188);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.Controls.Add(btnSaveKey, 1, 0);
            tableLayoutPanel5.Controls.Add(lbKey, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 94);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(844, 94);
            tableLayoutPanel5.TabIndex = 1;
            // 
            // btnSaveKey
            // 
            btnSaveKey.AutoSize = false;
            btnSaveKey.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSaveKey.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnSaveKey.Depth = 0;
            btnSaveKey.Dock = DockStyle.Fill;
            btnSaveKey.HighEmphasis = true;
            btnSaveKey.Icon = null;
            btnSaveKey.Location = new Point(679, 6);
            btnSaveKey.Margin = new Padding(4, 6, 4, 6);
            btnSaveKey.MouseState = MaterialSkin.MouseState.HOVER;
            btnSaveKey.Name = "btnSaveKey";
            btnSaveKey.NoAccentTextColor = Color.Empty;
            btnSaveKey.Size = new Size(161, 82);
            btnSaveKey.TabIndex = 7;
            btnSaveKey.Text = "儲存";
            btnSaveKey.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnSaveKey.UseAccentColor = false;
            btnSaveKey.UseVisualStyleBackColor = true;
            btnSaveKey.Click += btnSaveKey_Click;
            // 
            // lbKey
            // 
            lbKey.AnimateReadOnly = false;
            lbKey.AutoCompleteMode = AutoCompleteMode.None;
            lbKey.AutoCompleteSource = AutoCompleteSource.None;
            lbKey.BackgroundImageLayout = ImageLayout.None;
            lbKey.CharacterCasing = CharacterCasing.Normal;
            lbKey.Depth = 0;
            lbKey.Dock = DockStyle.Fill;
            lbKey.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbKey.HideSelection = true;
            lbKey.Hint = "Key (身分碼)";
            lbKey.LeadingIcon = null;
            lbKey.Location = new Point(6, 6);
            lbKey.Margin = new Padding(6);
            lbKey.MaxLength = 32767;
            lbKey.MouseState = MaterialSkin.MouseState.OUT;
            lbKey.Name = "lbKey";
            lbKey.PasswordChar = '\0';
            lbKey.PrefixSuffixText = null;
            lbKey.ReadOnly = false;
            lbKey.RightToLeft = RightToLeft.No;
            lbKey.SelectedText = "";
            lbKey.SelectionLength = 0;
            lbKey.SelectionStart = 0;
            lbKey.ShortcutsEnabled = true;
            lbKey.Size = new Size(663, 48);
            lbKey.TabIndex = 1;
            lbKey.TabStop = false;
            lbKey.TextAlign = HorizontalAlignment.Left;
            lbKey.TrailingIcon = null;
            lbKey.UseSystemPasswordChar = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Controls.Add(lbDataDir, 0, 0);
            tableLayoutPanel4.Controls.Add(btnSelectDir, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(844, 94);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // lbDataDir
            // 
            lbDataDir.AnimateReadOnly = false;
            lbDataDir.AutoCompleteMode = AutoCompleteMode.None;
            lbDataDir.AutoCompleteSource = AutoCompleteSource.None;
            lbDataDir.BackgroundImageLayout = ImageLayout.None;
            lbDataDir.CharacterCasing = CharacterCasing.Normal;
            lbDataDir.Depth = 0;
            lbDataDir.Dock = DockStyle.Fill;
            lbDataDir.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            lbDataDir.HideSelection = true;
            lbDataDir.Hint = "working-dir (數據路徑)";
            lbDataDir.LeadingIcon = null;
            lbDataDir.Location = new Point(6, 6);
            lbDataDir.Margin = new Padding(6);
            lbDataDir.MaxLength = 32767;
            lbDataDir.MouseState = MaterialSkin.MouseState.OUT;
            lbDataDir.Name = "lbDataDir";
            lbDataDir.PasswordChar = '\0';
            lbDataDir.PrefixSuffixText = null;
            lbDataDir.ReadOnly = true;
            lbDataDir.RightToLeft = RightToLeft.No;
            lbDataDir.SelectedText = "";
            lbDataDir.SelectionLength = 0;
            lbDataDir.SelectionStart = 0;
            lbDataDir.ShortcutsEnabled = true;
            lbDataDir.Size = new Size(663, 48);
            lbDataDir.TabIndex = 0;
            lbDataDir.TabStop = false;
            lbDataDir.TextAlign = HorizontalAlignment.Left;
            lbDataDir.TrailingIcon = null;
            lbDataDir.UseSystemPasswordChar = false;
            lbDataDir.DoubleClick += lbDataDir_DoubleClick;
            // 
            // btnSelectDir
            // 
            btnSelectDir.AutoSize = false;
            btnSelectDir.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSelectDir.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnSelectDir.Depth = 0;
            btnSelectDir.Dock = DockStyle.Fill;
            btnSelectDir.HighEmphasis = true;
            btnSelectDir.Icon = null;
            btnSelectDir.Location = new Point(679, 6);
            btnSelectDir.Margin = new Padding(4, 6, 4, 6);
            btnSelectDir.MouseState = MaterialSkin.MouseState.HOVER;
            btnSelectDir.Name = "btnSelectDir";
            btnSelectDir.NoAccentTextColor = Color.Empty;
            btnSelectDir.Size = new Size(161, 82);
            btnSelectDir.TabIndex = 1;
            btnSelectDir.Text = "選擇路徑";
            btnSelectDir.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnSelectDir.UseAccentColor = false;
            btnSelectDir.UseVisualStyleBackColor = true;
            btnSelectDir.Click += btnSelectDir_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(864, 860);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(materialTabControl1);
            DrawerTabControl = materialTabControl1;
            DrawerUseColors = true;
            Name = "Form1";
            Text = "Aron.Titan.Agent";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            materialTabControl1.ResumeLayout(false);
            tabOverview.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ImageList imageList1;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private TabPage tabOverview;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialTextBox2 lbDataDir;
        private MaterialSkin.Controls.MaterialTextBox2 lbKey;
        private MaterialSkin.Controls.MaterialButton btnSelectDir;
        private FolderBrowserDialog folderBrowserDialog1;
        private TableLayoutPanel tableLayoutPanel6;
        private MaterialSkin.Controls.MaterialTextBox2 lbWinVersion;
        private MaterialSkin.Controls.MaterialTextBox2 lbVm;
        private MaterialSkin.Controls.MaterialTextBox2 lbVirtualization;
        private MaterialSkin.Controls.MaterialTextBox2 lbMem;
        private MaterialSkin.Controls.MaterialTextBox2 lbDisk;
        private System.Windows.Forms.Timer timer1;
        private MaterialSkin.Controls.MaterialTextBox2 lbCPU;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel8;
        private MaterialSkin.Controls.MaterialTextBox2 lbServiceStatus;
        private MaterialSkin.Controls.MaterialButton btnInstallVM;
        private MaterialSkin.Controls.MaterialButton btnInstall;
        private MaterialSkin.Controls.MaterialButton btnStart;
        private MaterialSkin.Controls.MaterialButton btnSaveKey;
        private MaterialSkin.Controls.MaterialButton btnGoToDashboard;
        private MaterialSkin.Controls.MaterialButton btnInstallMultipass;
        private MaterialSkin.Controls.MaterialTextBox2 lbMultipassStatus;
        private MaterialSkin.Controls.MaterialButton btnLog;
        private MaterialSkin.Controls.MaterialButton btnStart1;
    }
}
