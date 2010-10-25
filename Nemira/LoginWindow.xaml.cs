using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nemira
{
    public partial class LoginWindow : Window
    {
        public event EventHandler LoginSuccessful;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            LoginSuccessful(this, null);
            Close();
        }

        public string Email
        {
            get { return email.Text; }
        }

        public string Password
        {
            get { return password.Password; }
        }
    }
}
