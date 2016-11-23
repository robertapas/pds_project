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
            tDirectory.Text = settingsManager.readSetting("account", "directory");
            tTimeout.Text = settingsManager.readSetting("connection", "syncTime");
        }

       /* private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Start the login procedure
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                // i have to create the connection in order to perform the login
                openLogin();
            }));
        }*/

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
                                this.ErrorMessage = "Registration faild";
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
                loggedin = false;
                tpLogin.IsSelected = true;
                tpHome.IsEnabled = false;
                tpSettings.IsEnabled = false;
                tpVersions.IsEnabled = false;
                
            }
            lVersions.Items.Clear();
            updateStatus("Logged Out");

        }

        /*private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, tUsername);
        }*/

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


        private void updateStatusBar(int percentage)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                lStatusBar.Value = percentage;
            }));
        }
    }
}
