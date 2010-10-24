using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Nemira
{
    public partial class App : Application
    {
        private LoginWindow login = new LoginWindow();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = login;

            login.LoginSuccessful += StartupMainWindow;
            login.Show();
        }

        private void StartupMainWindow(object sender, EventArgs e)
        {
            var main = new MainWindow();

            Application.Current.MainWindow = main;
            main.OpenAccount(login.Email, login.Password);
        }
    }
}
