using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public MainWindow()
        {
            InitializeComponent();
            lError.Content = "";
        }



        private async void openLogin()
        {
            //LoginWindow lw = new LoginWindow();
            bool loginAuthorized = false;
            bLogInOut.IsEnabled = false;
            lw.Username = settingsManager.readSetting("account", "username");
            lw.Username = settingsManager.readSetting("account", "password");
            while (!loginAuthorized)
            {
                lw.showLogin();
                try
                {
                    switch (lw.waitResponse())
                    {
                        case LoginWindow.LoginResponse.CANCEL:
                            //System.Windows.Application.Current.Shutdown();
                            lw.Close();
                            bLogInOut.IsEnabled = true;
                            return;
                        case LoginWindow.LoginResponse.LOGIN:
                            loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), lw.Username, lw.Password);
                            if (!loginAuthorized)
                            {
                                lw.ErrorMessage = "Login faild";
                            }
                            break;
                        case LoginWindow.LoginResponse.REGISTER:
                            loginAuthorized = await syncManager.login(tAddress.Text, Convert.ToInt32(tPort.Text), lw.Username, lw.Password, tDirectory.Text, true);
                            if (!loginAuthorized)
                            {
                                lw.ErrorMessage = "Registration faild";
                            }
                            break;
                        default:
                            throw new Exception("Not implemented");
                    }
                    if (loginAuthorized)
                    {
                        lUsername.Content = lw.Username;
                        bLogInOut.Content = "Logout";
                        lw.Close();
                        settingsManager.writeSetting("account", "username", lw.Username);
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
                    lw.ErrorMessage = ex.Message;
                    loginAuthorized = false;
                }
            }
            bLogInOut.IsEnabled = true;
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
                lError.Content = value;
            }
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            username = tUsername.Text;
            password = tPassword.Password;
            if (username == "" || password == "")
            {
                this.ErrorMessage = "Username and passwrod cannot be empty";
                return;
            }
            lastResponse = LoginResponse.LOGIN;
            this.Hide();
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
            this.Hide();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, tUsername);
        }


    }
}
