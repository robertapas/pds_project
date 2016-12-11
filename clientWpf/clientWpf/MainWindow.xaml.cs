using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace clientWpf
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public enum LoginResponse { LOGIN, REGISTER};
        private LoginResponse lastResponse;
        private String username, password;
        private bool loggedin = false;
        private SyncManager syncManager;
        private SettingsManager settingsManager;
        List<Version> versions = null;

        public MainWindow()
        {
            InitializeComponent();
            lError.Text= "";
            // initialize my data structure
            syncManager = new SyncManager();
            syncManager.setStatusDelegate(updateStatus, updateStatusBar);

            // Settings manager
            settingsManager = new SettingsManager();
            tAddress.Text = settingsManager.readSetting("connection", "address");
            tPort.Text = settingsManager.readSetting("connection", "port");
            //tDirectory.Text = settingsManager.readSetting("account", "directory");
            tTimeout.Text = settingsManager.readSetting("connection", "syncTime");
        }

        private async void login_register()
        {

            bool loginAuthorized = false;
            bLogOut.IsEnabled = false;
            //this.Username = settingsManager.readSetting("account", "username");
            //this.Password = settingsManager.readSetting("account", "password");
            
                try
                {
                    switch (waitResponse())
                    {
                        
                        case LoginResponse.LOGIN:
                            loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), this.Username, this.Password);
                            if (!loginAuthorized)
                            {
                                this.ErrorMessage = "Login failed";
                            }
                            break;
                        case LoginResponse.REGISTER:
                            loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), this.Username, this.Password, tDirectory.Text, true);
                            if (!loginAuthorized)
                            {
                                this.ErrorMessage = "Registration failed";
                            }
                            break;
                        default:
                            throw new Exception("Not implemented");
                    }
                    if (loginAuthorized)
                    {
                        tpHome.IsEnabled = true;
                        tpHome.IsSelected = true;
                        tpSettings.IsEnabled = true;
                        tpVersions.IsEnabled = true;
                        lUsername.Content = this.Username;
                        bLogOut.Content = "Logout";

                        //ABILITA PANNELLI
                        settingsManager.writeSetting("account", "username", this.Username);
                        /*if(lw.KeepLoggedIn)
							settingsManager.writeSetting("account", "password", lw.Username);
						else
							settingsManager.writeSetting("account", "password", "");
						*/
                        bStart.IsEnabled = true;
                        loggedin = true;
                        updateStatus("Logged in");
                        //StartSync_Click(null, null); // start sync
                    }
                }
                catch (Exception ex)
                {

                    this.ErrorMessage = ex.Message;
                    loginAuthorized = false;
                }
            
            bLogOut.IsEnabled = true;
        }

        private void updateStatus(String newStatus)
        {
            lStatus.Content = newStatus;
            ListBoxItem lbi = new ListBoxItem();
            lbi.Content = newStatus;
            lbStatus.Items.Add(lbi);
            lbStatus.SelectedIndex = lbStatus.Items.Count - 1;
            lbStatus.ScrollIntoView(lbStatus.SelectedIndex);
        }

        public LoginResponse waitResponse()
        {
            return lastResponse;
        }

        public String Username
        {
            set
            {
                username = value;
                tUsername.Text = value;
            }
            get
            {
                return username;
            }
        }
        public String Password
        {
            private set
            {
                password = value;
                tPassword.Password = value;
            }
            get
            {
                SHA256Managed hashstring = new SHA256Managed();
                return Encoding.ASCII.GetString(hashstring.ComputeHash(Encoding.ASCII.GetBytes(password + username)));
            }
        }
        public String ErrorMessage
        {
            set
            {
                lError.Text = value;
            }
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            lbStatus.Items.Clear();
            lbStatus.Items.Refresh();
            username = tUsername.Text;
            password = tPassword.Password;
            if (username == "" || password == "")
            {
                this.ErrorMessage = "Username and password cannot be empty";
                return;
            }
            lastResponse = LoginResponse.LOGIN;
            this.login_register();

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            lbStatus.Items.Clear();
            lbStatus.Items.Refresh();
            username = tUsername.Text;
            password = tPassword.Password;
            if (username == "" || password == "")
            {
                this.ErrorMessage = "Username and passwrod cannot be empty";
                return;
            }
            lastResponse = LoginResponse.REGISTER;
            this.login_register();

        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (loggedin)
            {
                forceStop();
                lUsername.Content = "";
                lbStatus.Items.Clear();
                lbStatus.Items.Refresh();

                loggedin = false;
                tpLogin.IsSelected = true;
                tpHome.IsEnabled = false;
                tpSettings.IsEnabled = false;
                tpVersions.IsEnabled = false;
                
            }
            lVersions.Items.Clear();
            updateStatus("Logged Out");

        }

        private void forceStop()
        {
            bStop.IsEnabled = false;
            bSyncNow.IsEnabled = false;
            bRestore.IsEnabled = false;
            bGetVersions.IsEnabled = false;
            syncManager.stopSync();
            bStart.IsEnabled = true;
            tDirectory.IsEnabled = true;
            tTimeout.IsEnabled = true;
            bBrowse.IsEnabled = true;
            tAddress.IsEnabled = true;
            tPort.IsEnabled = true;
        }

        private void updateStatus(String message, bool fatalError)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                updateStatus(message);
                if (fatalError)
                {
                    forceStop();
                }
            }));
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder to synchronize";
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            // start the sync manager
            try
            {
                bStart.IsEnabled = false;
                lVersions.Items.Clear();
                syncManager.startSync(tAddress.Text, Int32.Parse(tPort.Text), tDirectory.Text, Int32.Parse(tTimeout.Text) * 1000);
                bStop.IsEnabled = true;
                bSyncNow.IsEnabled = true;
                bGetVersions.IsEnabled = true;
                tDirectory.IsEnabled = false;
                tTimeout.IsEnabled = false;
                bBrowse.IsEnabled = false;
                tAddress.IsEnabled = false;
                tPort.IsEnabled = false;
                updateStatus("Started");
                // Save settings
                settingsManager.writeSetting("connection", "address", tAddress.Text);
                settingsManager.writeSetting("connection", "port", tPort.Text);
                //settingsManager.writeSetting("account", "directory", tDirectory.Text);
                settingsManager.writeSetting("connection", "syncTime", tTimeout.Text);
            }
            catch (Exception ex)
            {
                bStart.IsEnabled = true;
                updateStatus(ex.Message);
            }
        }

        private void StopSync_Click(object sender, RoutedEventArgs e)
        {
            // stop the sync manager
            try
            {
                lVersions.Items.Clear();
                updateStatus("Stop");
                forceStop();
            }
            catch (Exception ex)
            {
                bStop.IsEnabled = true;
                bSyncNow.IsEnabled = true;
                updateStatus(ex.Message);
            }
        }

        private void updateStatusBar(int percentage)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                lStatusBar.Value = percentage;
            }));
        }

        private void lVersions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (obj != null && obj != lVersions)
            {
                if (obj.GetType() == typeof(System.Windows.Controls.ListViewItem))
                {
                    VersionDetailsWindow vdw = new VersionDetailsWindow(versions[lVersions.SelectedIndex], syncManager);
                    vdw.Show();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private async void bGetVersions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bGetVersions.IsEnabled = false;
                lVersions.Items.Clear();
                lVersions.Items.Refresh();
                versions = await syncManager.getVersions();
                foreach (Version version in versions)
                {
                    lVersions.Items.Add(new VersionsListViewItem(version.VersionNum, version.NewFiles, version.EditFiles, version.DelFiles, version.Timestamp));
                }
                lVersions.SelectedIndex = 0;

                bGetVersions.IsEnabled = true;
                bRestore.IsEnabled = true;
            }
            catch (Exception ex)
            {
                bGetVersions.IsEnabled = true;
                updateStatus(ex.Message);
            }
        }

        private async void Restore_Click(object sender, RoutedEventArgs e)
        {
            bRestore.IsEnabled = false;
            Int64 selVersion = versions[lVersions.SelectedIndex].VersionNum;
            MessageBoxResult res = System.Windows.MessageBox.Show("Do you want to restore version number " + selVersion + " ?", "Restore system", System.Windows.MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    await syncManager.restoreVersion(selVersion);
                    System.Windows.MessageBox.Show("Restore Done!", "Restoring system");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Restore failed:\n" + ex.Message, "Restoring system", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            bRestore.IsEnabled = true;
        }

        private void bSyncNow_Click(object sender, RoutedEventArgs e)
    {
            syncManager.forceSync();
    }

        
    }

    class VersionsListViewItem
    {
        public String sVersion { get; set; }
        public String sNewFiles { get; set; }
        public String sEditFiles { get; set; }
        public String sDelFiles { get; set; }
        public String sDateTime { get; set; }
        public VersionsListViewItem(Int64 version, int newFiles, int editFiles, int delFiles, String dateTime)
        {
            sVersion = version.ToString();
            sNewFiles = newFiles.ToString();
            sEditFiles = editFiles.ToString();
            sDelFiles = delFiles.ToString();
            sDateTime = dateTime;
        }
    }
}
