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
        private String[] settings = new String[3]; //port, 
        private SettingsManager settingsManager;
        private AsyncManagerServer syncManager;
        private delegate void AppendItem(String s);

        public fSyncServer()
        {
            InitializeComponent();
            syncManager = new AsyncManagerServer(appendStatus, numberClient);
            settingsManager = new SettingsManager();
        }

        private void fSyncServer_Load(object sender, EventArgs e)
        {
            settings = settingsManager.readSettings();
            nPort.Value = Int32.Parse(settings[0]);
            tDirectory.Text = settings[1];
            nUDVersion.Value = Int32.Parse(settings[2]);
        }


        private void bBrowse_Click(object sender, EventArgs e)
        {
            /*Select folder to store files (mock DB)*/
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
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
            /*update num connected client info*/
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

        private void bStartClick(object sender, EventArgs e)
        {
            try
            {
                syncManager.startSync(Decimal.ToInt32(Int32.Parse(settings[0])), settings[1], Decimal.ToInt32(Int32.Parse(settings[2])));
                tDirectory.Enabled = false;
                nPort.Enabled = false;
                bStart.Enabled = false;
                nUDVersion.Enabled = false;
                bStop.Enabled = true;
            }
            catch (Exception ex)
            {
                this.appendStatus(ex.Message, LOG_ERROR);
            }
        }

        private void nPort_ValueChanged(object sender, EventArgs e)
        {
            settings[0] = nPort.Value.ToString();
        }

        private void nUDVersion_ValueChanged(object sender, EventArgs e)
        {

            settings[2] = nUDVersion.Value.ToString();
        }


        private void bStop_Click(object sender, EventArgs e)
        {
            syncManager.stopSync();
            tDirectory.Enabled = true;
            nPort.Enabled = true;
            bStart.Enabled = true;
            nUDVersion.Enabled = true;
            bStop.Enabled = false;
        }

        private void bDel_Click(object sender, EventArgs e)
        {

            SyncSQLite mySQLite;
            mySQLite = new SyncSQLite();

            ListView.SelectedListViewItemCollection users = lvUsers.SelectedItems;
            ListViewItem item = null;
            if (users.Count != 0)
                item = users[0];
            if (item != null)
            {
                Int64 userID = Int64.Parse(item.SubItems[0].Text);
                mySQLite.deleteUser(userID);
            }
            if (lvUsers.Items.Count != 0)
                bUsers_Click(null, null);
            mySQLite.closeConnection();
        }

        private void bUsers_Click(object sender, EventArgs e)
        {
            lvUsers.Items.Clear();
            SyncSQLite mySQLite;
            List<SyncSQLite.UserVersions> myUserVersion;
            mySQLite = new SyncSQLite();
            myUserVersion = mySQLite.getUsersList();
            foreach (SyncSQLite.UserVersions user in myUserVersion)
            {
                lvUsers.Items.Add(new ListViewItem(new String[] { user.userId.ToString(), user.username, user.versionCount.ToString() }));
            }
            mySQLite.closeConnection();
            lvVersions.Items.Clear();
        }

        private void lvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lvVersions.Items.Clear();
            SyncSQLite mySQLite;
            ListViewItem item = null;
            Int64 userID;
            Int64 maxVers = 0;
            Int64 minVers;
            mySQLite = new SyncSQLite();

            ListView.SelectedListViewItemCollection user = lvUsers.SelectedItems;
            if (user.Count != 0)
                item = user[0];
            if (item != null)
            {
                userID = Int64.Parse(item.SubItems[0].Text);
                minVers = mySQLite.getUserMinMaxVersion(userID, ref maxVers);
                if ((minVers == 0) && (maxVers == 0))
                {
                    lvVersions.Items.Add(new ListViewItem(new String[] { "ANY", "ANY", "ANY" }));
                }
                else
                {
                    while (minVers <= maxVers)
                    {
                        List<FileChecksum> files = mySQLite.getUserFiles(userID, minVers, "");
                        if (files.Count != 0)
                            lvVersions.Items.Add(new ListViewItem(new String[] { minVers.ToString(), files.Count.ToString(), files[0].Timestamp }));
                        minVers++;
                    }
                }
            }
            mySQLite.closeConnection();
        }


    }
}
