using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nemira
{
    public partial class RenameSubscription : Window
    {
        public RenameSubscription()
        {
            InitializeComponent();
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string SubscriptionName
        {
            get { return name.Text; }
            set { name.Text = value; }
        }
    }
}
