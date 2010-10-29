using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GoogleReader.API;

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
            var account = new ReaderAccount(login.Email, login.Password);
            var main = new MainWindow(account);

            Application.Current.MainWindow = main;

            main.OpenAccount();
        }
    }
}
