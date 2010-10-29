using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GoogleReader.API;

namespace Nemira
{
    public partial class RenameDialog : Window
    {
        public RenameDialog(Subscription subscription)
        {
            InitializeComponent();

            SubscriptionName = subscription.Title;

            Loaded += new RoutedEventHandler((s, e) => {
                name.SelectAll();
            });
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string SubscriptionName
        {
            get { return name.Text; }
            private set { name.Text = value; }
        }
    }
}
