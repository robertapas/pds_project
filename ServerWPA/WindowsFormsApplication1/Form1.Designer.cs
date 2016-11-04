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
            this.tVersions = new System.Windows.Forms.TabPage();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.lPort = new System.Windows.Forms.Label();
            this.tDirectory = new System.Windows.Forms.TextBox();
            this.bBrowse = new System.Windows.Forms.Button();
            this.lDirectory = new System.Windows.Forms.Label();
            this.lLog = new System.Windows.Forms.Label();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.bStart = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.lConnectedUser = new System.Windows.Forms.Label();
            this.lConnectedNum = new System.Windows.Forms.Label();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.cId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvVersions = new System.Windows.Forms.ListView();
            this.cVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTotFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lUsers = new System.Windows.Forms.Label();
            this.lVersions = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tSettings.SuspendLayout();
            this.tVersions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tSettings);
            this.tabControl1.Controls.Add(this.tVersions);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(915, 389);
            this.tabControl1.TabIndex = 0;
            // 
            // tSettings
            // 
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
            this.tSettings.Location = new System.Drawing.Point(4, 25);
            this.tSettings.Name = "tSettings";
            this.tSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tSettings.Size = new System.Drawing.Size(907, 360);
            this.tSettings.TabIndex = 0;
            this.tSettings.Text = "Settings";
            this.tSettings.UseVisualStyleBackColor = true;
            // 
            // tVersions
            // 
            this.tVersions.Controls.Add(this.lVersions);
            this.tVersions.Controls.Add(this.lUsers);
            this.tVersions.Controls.Add(this.lvVersions);
            this.tVersions.Controls.Add(this.lvUsers);
            this.tVersions.Location = new System.Drawing.Point(4, 25);
            this.tVersions.Name = "tVersions";
            this.tVersions.Size = new System.Drawing.Size(907, 360);
            this.tVersions.TabIndex = 2;
            this.tVersions.Text = "Versions";
            this.tVersions.UseVisualStyleBackColor = true;
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(30, 42);
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(120, 22);
            this.nPort.TabIndex = 0;
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(30, 19);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(79, 17);
            this.lPort.TabIndex = 1;
            this.lPort.Text = "Server port";
            // 
            // tDirectory
            // 
            this.tDirectory.Location = new System.Drawing.Point(172, 41);
            this.tDirectory.Name = "tDirectory";
            this.tDirectory.Size = new System.Drawing.Size(601, 22);
            this.tDirectory.TabIndex = 2;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(780, 41);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 3;
            this.bBrowse.Text = "...";
            this.bBrowse.UseVisualStyleBackColor = true;
            // 
            // lDirectory
            // 
            this.lDirectory.AutoSize = true;
            this.lDirectory.Location = new System.Drawing.Point(169, 21);
            this.lDirectory.Name = "lDirectory";
            this.lDirectory.Size = new System.Drawing.Size(119, 17);
            this.lDirectory.TabIndex = 4;
            this.lDirectory.Text = "Working directory";
            // 
            // lLog
            // 
            this.lLog.AutoSize = true;
            this.lLog.Location = new System.Drawing.Point(30, 104);
            this.lLog.Name = "lLog";
            this.lLog.Size = new System.Drawing.Size(95, 17);
            this.lLog.TabIndex = 5;
            this.lLog.Text = "Messages log";
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 16;
            this.lbLog.Location = new System.Drawing.Point(33, 138);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(822, 84);
            this.lbLog.TabIndex = 6;
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(74, 280);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(75, 23);
            this.bStart.TabIndex = 7;
            this.bStart.Text = "Start server";
            this.bStart.UseVisualStyleBackColor = true;
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(172, 280);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(75, 23);
            this.bStop.TabIndex = 8;
            this.bStop.Text = "Stop Server";
            this.bStop.UseVisualStyleBackColor = true;
            // 
            // lConnectedUser
            // 
            this.lConnectedUser.AutoSize = true;
            this.lConnectedUser.Location = new System.Drawing.Point(506, 280);
            this.lConnectedUser.Name = "lConnectedUser";
            this.lConnectedUser.Size = new System.Drawing.Size(115, 17);
            this.lConnectedUser.TabIndex = 9;
            this.lConnectedUser.Text = "Connected users";
            // 
            // lConnectedNum
            // 
            this.lConnectedNum.AutoSize = true;
            this.lConnectedNum.Location = new System.Drawing.Point(618, 280);
            this.lConnectedNum.Name = "lConnectedNum";
            this.lConnectedNum.Size = new System.Drawing.Size(16, 17);
            this.lConnectedNum.TabIndex = 10;
            this.lConnectedNum.Text = "0";
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
            this.lvUsers.Location = new System.Drawing.Point(37, 35);
            this.lvUsers.Margin = new System.Windows.Forms.Padding(4);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(789, 132);
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
            this.lvVersions.Location = new System.Drawing.Point(28, 222);
            this.lvVersions.Margin = new System.Windows.Forms.Padding(4);
            this.lvVersions.Name = "lvVersions";
            this.lvVersions.Size = new System.Drawing.Size(789, 123);
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
            // lUsers
            // 
            this.lUsers.AutoSize = true;
            this.lUsers.Location = new System.Drawing.Point(34, 14);
            this.lUsers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lUsers.Name = "lUsers";
            this.lUsers.Size = new System.Drawing.Size(49, 17);
            this.lUsers.TabIndex = 4;
            this.lUsers.Text = "Users:";
            // 
            // lVersions
            // 
            this.lVersions.AutoSize = true;
            this.lVersions.Location = new System.Drawing.Point(34, 201);
            this.lVersions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lVersions.Name = "lVersions";
            this.lVersions.Size = new System.Drawing.Size(63, 17);
            this.lVersions.TabIndex = 5;
            this.lVersions.Text = "Versions";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 394);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tSettings.ResumeLayout(false);
            this.tSettings.PerformLayout();
            this.tVersions.ResumeLayout(false);
            this.tVersions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
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
    }
}

