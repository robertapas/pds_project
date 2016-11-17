﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class fSyncServer : Form
    {

        public const int LOG_NORMAL = 0;
        public const int LOG_INFO = 1;
        public const int LOG_WARNING = 2;
        public const int LOG_ERROR = 3;
        private String[] settings = new String[3];
        private SettingsManager settingsManager;
        private AsyncManagerServer syncManager;
        private delegate void AppendItem(String s);

        public fSyncServer()
        {
            InitializeComponent();
            syncManager = new AsyncManagerServer(appendStatus, numberClient);
            settingsManager = new SettingsManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        private void bBrowse_Click(object sender, EventArgs e)
        {
            /*Select folder to store files (mock DB)*/
            FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the server working directory";
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tDirectory.Text = folderBrowserDialog.SelectedPath;
            }
            settings[1] = tDirectory.Text;
        }

        private void numberClient(int nclient)
        {
            lConnectedNum.BeginInvoke(new Action(() => { lConnectedNum.Text = nclient.ToString(); }));
        }

        private void appendStatus(String s, int type = LOG_NORMAL)
        {
            /*update log info*/
            switch (type)
            {
                case LOG_INFO:
                    s = "INFO: " + s;
                    break;
                case LOG_WARNING:
                    s = "WARNING: " + s;
                    break;
                case LOG_ERROR:
                    s = "ERROR: " + s;
                    break;
            }

            // Send the command to add a new item on the listbox on the UI thread
            lbLog.BeginInvoke(new AppendItem((String str) => { lbLog.Items.Add(str); }), new object[] { s });
            lbLog.BeginInvoke(new Action(() => { lbLog.SelectedIndex = lbLog.Items.Count - 1; }));
        }

    }
}
