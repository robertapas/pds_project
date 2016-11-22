namespace WindowsFormsApplication1
{
    partial class fSyncServer
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tSettings = new System.Windows.Forms.TabPage();
            this.nUDVersion = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lConnectedNum = new System.Windows.Forms.Label();
            this.lConnectedUser = new System.Windows.Forms.Label();
            this.bStop = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.lLog = new System.Windows.Forms.Label();
            this.lDirectory = new System.Windows.Forms.Label();
            this.bBrowse = new System.Windows.Forms.Button();
            this.tDirectory = new System.Windows.Forms.TextBox();
            this.lPort = new System.Windows.Forms.Label();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.tVersions = new System.Windows.Forms.TabPage();
            this.lVersions = new System.Windows.Forms.Label();
            this.lUsers = new System.Windows.Forms.Label();
            this.lvVersions = new System.Windows.Forms.ListView();
            this.cVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTotFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvUsers = new System.Windows.Forms.ListView();
            this.cId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.tVersions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tSettings);
            this.tabControl1.Controls.Add(this.tVersions);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 316);
            this.tabControl1.TabIndex = 0;
            // 
            // tSettings
            // 
            this.tSettings.Controls.Add(this.nUDVersion);
            this.tSettings.Controls.Add(this.label1);
            this.tSettings.Controls.Add(this.lConnectedNum);
            this.tSettings.Controls.Add(this.lConnectedUser);
            this.tSettings.Controls.Add(this.bStop);
            this.tSettings.Controls.Add(this.bStart);
            this.tSettings.Controls.Add(this.lbLog);
            this.tSettings.Controls.Add(this.lLog);
            this.tSettings.Controls.Add(this.lDirectory);
            this.tSettings.Controls.Add(this.bBrowse);
            this.tSettings.Controls.Add(this.tDirectory);
            this.tSettings.Controls.Add(this.lPort);
            this.tSettings.Controls.Add(this.nPort);
            this.tSettings.Location = new System.Drawing.Point(4, 22);
            this.tSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tSettings.Name = "tSettings";
            this.tSettings.Padding = new System.Windows.Forms.Padding(2);
            this.tSettings.Size = new System.Drawing.Size(678, 290);
            this.tSettings.TabIndex = 0;
            this.tSettings.Text = "Settings";
            this.tSettings.UseVisualStyleBackColor = true;
            this.tSettings.Click += new System.EventHandler(this.tSettings_Click);
            // 
            // nUDVersion
            // 
            this.nUDVersion.Location = new System.Drawing.Point(132, 99);
            this.nUDVersion.Margin = new System.Windows.Forms.Padding(2);
            this.nUDVersion.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nUDVersion.Name = "nUDVersion";
            this.nUDVersion.Size = new System.Drawing.Size(80, 20);
            this.nUDVersion.TabIndex = 23;
            this.nUDVersion.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Max Version Number:";
            // 
            // lConnectedNum
            // 
            this.lConnectedNum.AutoSize = true;
            this.lConnectedNum.Location = new System.Drawing.Point(630, 247);
            this.lConnectedNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lConnectedNum.Name = "lConnectedNum";
            this.lConnectedNum.Size = new System.Drawing.Size(13, 13);
            this.lConnectedNum.TabIndex = 10;
            this.lConnectedNum.Text = "0";
            // 
            // lConnectedUser
            // 
            this.lConnectedUser.AutoSize = true;
            this.lConnectedUser.Location = new System.Drawing.Point(539, 247);
            this.lConnectedUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lConnectedUser.Name = "lConnectedUser";
            this.lConnectedUser.Size = new System.Drawing.Size(87, 13);
            this.lConnectedUser.TabIndex = 9;
            this.lConnectedUser.Text = "Connected users";
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(129, 244);
            this.bStop.Margin = new System.Windows.Forms.Padding(2);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(56, 19);
            this.bStop.TabIndex = 8;
            this.bStop.Text = "Stop Server";
            this.bStop.UseVisualStyleBackColor = true;
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(25, 244);
            this.bStart.Margin = new System.Windows.Forms.Padding(2);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(56, 19);
            this.bStart.TabIndex = 7;
            this.bStart.Text = "Start server";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStartClick);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(25, 155);
            this.lbLog.Margin = new System.Windows.Forms.Padding(2);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(618, 69);
            this.lbLog.TabIndex = 6;
            // 
            // lLog
            // 
            this.lLog.AutoSize = true;
            this.lLog.Location = new System.Drawing.Point(22, 140);
            this.lLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lLog.Name = "lLog";
            this.lLog.Size = new System.Drawing.Size(72, 13);
            this.lLog.TabIndex = 5;
            this.lLog.Text = "Messages log";
            // 
            // lDirectory
            // 
            this.lDirectory.AutoSize = true;
            this.lDirectory.Location = new System.Drawing.Point(22, 68);
            this.lDirectory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lDirectory.Name = "lDirectory";
            this.lDirectory.Size = new System.Drawing.Size(93, 13);
            this.lDirectory.TabIndex = 4;
            this.lDirectory.Text = "Working directory:";
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(587, 65);
            this.bBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(56, 19);
            this.bBrowse.TabIndex = 3;
            this.bBrowse.Text = "...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // tDirectory
            // 
            this.tDirectory.Location = new System.Drawing.Point(132, 65);
            this.tDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.tDirectory.Name = "tDirectory";
            this.tDirectory.Size = new System.Drawing.Size(452, 20);
            this.tDirectory.TabIndex = 2;
            this.tDirectory.Text = "C:\\Users\\Roberta\\progetto_pds";
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(22, 15);
            this.lPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(59, 13);
            this.lPort.TabIndex = 1;
            this.lPort.Text = "Server port";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(22, 34);
            this.nPort.Margin = new System.Windows.Forms.Padding(2);
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(90, 20);
            this.nPort.TabIndex = 0;
            this.nPort.ValueChanged += new System.EventHandler(this.nPort_ValueChanged);
            // 
            // tVersions
            // 
            this.tVersions.Controls.Add(this.lVersions);
            this.tVersions.Controls.Add(this.lUsers);
            this.tVersions.Controls.Add(this.lvVersions);
            this.tVersions.Controls.Add(this.lvUsers);
            this.tVersions.Location = new System.Drawing.Point(4, 22);
            this.tVersions.Margin = new System.Windows.Forms.Padding(2);
            this.tVersions.Name = "tVersions";
            this.tVersions.Size = new System.Drawing.Size(678, 290);
            this.tVersions.TabIndex = 2;
            this.tVersions.Text = "Versions";
            this.tVersions.UseVisualStyleBackColor = true;
            // 
            // lVersions
            // 
            this.lVersions.AutoSize = true;
            this.lVersions.Location = new System.Drawing.Point(26, 163);
            this.lVersions.Name = "lVersions";
            this.lVersions.Size = new System.Drawing.Size(47, 13);
            this.lVersions.TabIndex = 5;
            this.lVersions.Text = "Versions";
            // 
            // lUsers
            // 
            this.lUsers.AutoSize = true;
            this.lUsers.Location = new System.Drawing.Point(26, 11);
            this.lUsers.Name = "lUsers";
            this.lUsers.Size = new System.Drawing.Size(37, 13);
            this.lUsers.TabIndex = 4;
            this.lUsers.Text = "Users:";
            // 
            // lvVersions
            // 
            this.lvVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvVersions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cVers,
            this.cTotFile,
            this.cTStamp});
            this.lvVersions.FullRowSelect = true;
            this.lvVersions.Location = new System.Drawing.Point(21, 180);
            this.lvVersions.Name = "lvVersions";
            this.lvVersions.Size = new System.Drawing.Size(593, 101);
            this.lvVersions.TabIndex = 3;
            this.lvVersions.UseCompatibleStateImageBehavior = false;
            this.lvVersions.View = System.Windows.Forms.View.Details;
            // 
            // cVers
            // 
            this.cVers.Text = "Version";
            this.cVers.Width = 100;
            // 
            // cTotFile
            // 
            this.cTotFile.Text = "Total Files";
            this.cTotFile.Width = 100;
            // 
            // cTStamp
            // 
            this.cTStamp.Text = "TimeStamp";
            this.cTStamp.Width = 100;
            // 
            // lvUsers
            // 
            this.lvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cId,
            this.cUsername,
            this.cVersion});
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.Location = new System.Drawing.Point(21, 27);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(593, 108);
            this.lvUsers.TabIndex = 2;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            // 
            // cId
            // 
            this.cId.Text = "ID";
            // 
            // cUsername
            // 
            this.cUsername.Tag = "";
            this.cUsername.Text = "Username";
            this.cUsername.Width = 100;
            // 
            // cVersion
            // 
            this.cVersion.Text = "Version";
            this.cVersion.Width = 80;
            // 
            // fSyncServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 320);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "fSyncServer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tSettings.ResumeLayout(false);
            this.tSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.tVersions.ResumeLayout(false);
            this.tVersions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tSettings;
        private System.Windows.Forms.Label lConnectedNum;
        private System.Windows.Forms.Label lConnectedUser;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Label lLog;
        private System.Windows.Forms.Label lDirectory;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.TextBox tDirectory;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.NumericUpDown nPort;
        private System.Windows.Forms.TabPage tVersions;
        private System.Windows.Forms.Label lVersions;
        private System.Windows.Forms.Label lUsers;
        private System.Windows.Forms.ListView lvVersions;
        private System.Windows.Forms.ColumnHeader cVers;
        private System.Windows.Forms.ColumnHeader cTotFile;
        private System.Windows.Forms.ColumnHeader cTStamp;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.ColumnHeader cId;
        private System.Windows.Forms.ColumnHeader cUsername;
        private System.Windows.Forms.ColumnHeader cVersion;
        private System.Windows.Forms.NumericUpDown nUDVersion;
        private System.Windows.Forms.Label label1;
    }
}

