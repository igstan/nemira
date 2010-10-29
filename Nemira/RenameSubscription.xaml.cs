using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GoogleReader.API;

namespace Nemira
{
    public partial class RenameSubscription : Window
    {
        public RenameSubscription(Subscription subscription)
        {
            InitializeComponent();

            SubscriptionName = subscription.Title;
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
