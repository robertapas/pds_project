namespace ServerWFA
{
    partial class Form1
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
            this.bStart = new System.Windows.Forms.Button();
            this.bStop = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LabConnUser = new System.Windows.Forms.Label();
            this.LabConnUserNum = new System.Windows.Forms.Label();
            this.LabServerPort = new System.Windows.Forms.Label();
            this.LabWorkDire = new System.Windows.Forms.Label();
            this.textBrowse = new System.Windows.Forms.TextBox();
            this.bBrowse = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lUsers = new System.Windows.Forms.Label();
            this.lVersions = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.cId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvVersions = new System.Windows.Forms.ListView();
            this.cVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTotFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(29, 88);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(107, 40);
            this.bStart.TabIndex = 0;
            this.bStart.Text = "Start Server";
            this.bStart.UseVisualStyleBackColor = true;
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(142, 87);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(134, 41);
            this.bStop.TabIndex = 1;
            this.bStop.Text = "Stop Server";
            this.bStop.UseVisualStyleBackColor = true;
            // 
            // LogBox
            // 
            this.LogBox.FormattingEnabled = true;
            this.LogBox.ItemHeight = 16;
            this.LogBox.Location = new System.Drawing.Point(29, 134);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(742, 164);
            this.LogBox.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(795, 363);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.bBrowse);
            this.tabPage1.Controls.Add(this.textBrowse);
            this.tabPage1.Controls.Add(this.LabWorkDire);
            this.tabPage1.Controls.Add(this.LabServerPort);
            this.tabPage1.Controls.Add(this.LabConnUserNum);
            this.tabPage1.Controls.Add(this.LabConnUser);
            this.tabPage1.Controls.Add(this.LogBox);
            this.tabPage1.Controls.Add(this.bStart);
            this.tabPage1.Controls.Add(this.bStop);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(787, 312);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvVersions);
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Controls.Add(this.lVersions);
            this.tabPage2.Controls.Add(this.lUsers);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(787, 334);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Versions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LabConnUser
            // 
            this.LabConnUser.AutoSize = true;
            this.LabConnUser.Location = new System.Drawing.Point(580, 110);
            this.LabConnUser.Name = "LabConnUser";
            this.LabConnUser.Size = new System.Drawing.Size(118, 17);
            this.LabConnUser.TabIndex = 3;
            this.LabConnUser.Text = "Connected User: ";
            // 
            // LabConnUserNum
            // 
            this.LabConnUserNum.AutoSize = true;
            this.LabConnUserNum.Location = new System.Drawing.Point(704, 111);
            this.LabConnUserNum.Name = "LabConnUserNum";
            this.LabConnUserNum.Size = new System.Drawing.Size(16, 17);
            this.LabConnUserNum.TabIndex = 4;
            this.LabConnUserNum.Text = "0";
            // 
            // LabServerPort
            // 
            this.LabServerPort.AutoSize = true;
            this.LabServerPort.Location = new System.Drawing.Point(26, 22);
            this.LabServerPort.Name = "LabServerPort";
            this.LabServerPort.Size = new System.Drawing.Size(80, 17);
            this.LabServerPort.TabIndex = 5;
            this.LabServerPort.Text = "Server Port";
            // 
            // LabWorkDire
            // 
            this.LabWorkDire.AutoSize = true;
            this.LabWorkDire.Location = new System.Drawing.Point(226, 21);
            this.LabWorkDire.Name = "LabWorkDire";
            this.LabWorkDire.Size = new System.Drawing.Size(119, 17);
            this.LabWorkDire.TabIndex = 6;
            this.LabWorkDire.Text = "Working directory";
            // 
            // textBrowse
            // 
            this.textBrowse.Location = new System.Drawing.Point(229, 42);
            this.textBrowse.Name = "textBrowse";
            this.textBrowse.Size = new System.Drawing.Size(447, 22);
            this.textBrowse.TabIndex = 7;
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(683, 42);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 8;
            this.bBrowse.Text = "Browse";
            this.bBrowse.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(29, 42);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown1.TabIndex = 9;
            // 
            // lUsers
            // 
            this.lUsers.AutoSize = true;
            this.lUsers.Location = new System.Drawing.Point(21, 16);
            this.lUsers.Name = "lUsers";
            this.lUsers.Size = new System.Drawing.Size(45, 17);
            this.lUsers.TabIndex = 2;
            this.lUsers.Text = "Users";
            // 
            // lVersions
            // 
            this.lVersions.AutoSize = true;
            this.lVersions.Location = new System.Drawing.Point(21, 158);
            this.lVersions.Name = "lVersions";
            this.lVersions.Size = new System.Drawing.Size(63, 17);
            this.lVersions.TabIndex = 3;
            this.lVersions.Text = "Versions";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cId,
            this.cUsername,
            this.cVersion});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(21, 37);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(733, 117);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            this.lvVersions.Location = new System.Drawing.Point(24, 179);
            this.lvVersions.Margin = new System.Windows.Forms.Padding(4);
            this.lvVersions.Name = "lvVersions";
            this.lvVersions.Size = new System.Drawing.Size(730, 148);
            this.lvVersions.TabIndex = 5;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 377);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "SyncServer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.ListBox LogBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label LabConnUser;
        private System.Windows.Forms.Label LabConnUserNum;
        private System.Windows.Forms.Label LabServerPort;
        private System.Windows.Forms.Label LabWorkDire;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.TextBox textBrowse;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lVersions;
        private System.Windows.Forms.Label lUsers;
        private System.Windows.Forms.ListView lvVersions;
        private System.Windows.Forms.ColumnHeader cVers;
        private System.Windows.Forms.ColumnHeader cTotFile;
        private System.Windows.Forms.ColumnHeader cTStamp;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader cId;
        private System.Windows.Forms.ColumnHeader cUsername;
        private System.Windows.Forms.ColumnHeader cVersion;
    }
}

