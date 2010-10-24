using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoogleReader.API;
using System.Diagnostics;

namespace Nemira
{
    public partial class MainWindow : Window
    {
        private ReaderAccount readerAccount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PopulateContentPane(string content)
        {
            var html = @"<!DOCTYPE html>
            <html>
            <head>
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                <style type=""text/css"">
                img {
                    border: none;
                }
                p img {
                    float: right;
                }
                body {
                    font-size: 90%;
                    font-family: Tahoma, Verdana, Sans-Serif;
                }
                </style>
            </head>
            <body>" + content + "</body></html>";

            contentArea.NavigateToString(html);
        }

        public void OpenAccount(string email, string pass)
        {
            this.readerAccount = new ReaderAccount(email, pass);

            PopulateSubscriptionsTree();
            Show();
        }

        private void PopulateSubscriptionsTree()
        {
            subscriptions.ItemsSource = new Subscriptions(readerAccount);
        }

        private void OnAddFeed(object sender, RoutedEventArgs e)
        {
            // feedCategories.ElementAt(0).Feeds.Add(new Feed(feedUrl.Text, "http://someaddress.com/"));
        }

        private void OnSelectedFeed(object sender, RoutedEventArgs e)
        {
            if (subscriptions.SelectedItem is Subscription)
            {
                var subscription = subscriptions.SelectedItem as Subscription;

                editSubscription.IsEnabled = true;
                deleteSubscription.IsEnabled = true;

                feedTitle.Text = subscription.Title;
                feedItemTooltip.Content = subscription.SourceUrl;
                sourceHyperlink.NavigateUri = new Uri(subscription.SourceUrl);
            }

            if (subscriptions.SelectedItem is SubscriptionItem)
            {
                var item = subscriptions.SelectedItem as SubscriptionItem;

                editSubscription.IsEnabled = false;
                deleteSubscription.IsEnabled = false;

                feedTitle.Text = item.Title;
                feedItemTooltip.Content = "Open in default Web browser";
                sourceHyperlink.NavigateUri = new Uri(item.SourceUrl);

                PopulateContentPane(item.Content);
            }
        }

        private void OnUnselectedFeed(object sender, RoutedEventArgs e)
        {
            editSubscription.IsEnabled = false;
            deleteSubscription.IsEnabled = false;
        }

        private void OnExpandedSubscription(object sender, RoutedEventArgs e)
        {
            //var subscription = e.OriginalSource as Subscription;
        }

        private void OpenHyperlink(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void OnHyperlinkMouseEnter(object sender, RoutedEventArgs e)
        {
            var uri = ((Hyperlink)e.OriginalSource).NavigateUri;

            if (uri != null)
            {
                status.Text = uri.AbsoluteUri;
            }
        }

        private void OnHyperlinkMouseLeave(object sender, RoutedEventArgs e)
        {
            status.Text = "Done";
        }

        private void OnAddSubscription(object sender, RoutedEventArgs e)
        {
            var addSubscriptionDialog = new AddSubscription();
            addSubscriptionDialog.Owner = this;

            if (addSubscriptionDialog.ShowDialog() == true)
            {
                readerAccount.AddSubscription(addSubscriptionDialog.FeedUrl);
                subscriptions.ItemsSource = new Subscriptions(readerAccount);
            }
        }

        private void OnDeleteSubscription(object sender, RoutedEventArgs e)
        {
            var subscription = subscriptions.SelectedItem as Subscription;
            var message = String.Format("Unsubscribe from {0}?", subscription.Title);
            var title = "Confirm Unsubscribe";
            var button = MessageBoxButton.OKCancel;
            var icon = MessageBoxImage.Warning;

            if (MessageBox.Show(message, title, button, icon) == MessageBoxResult.OK) {
                readerAccount.RemoveSubscription(subscription);
                subscriptions.ItemsSource = new Subscriptions(readerAccount);
            }
        }
    }

    public class Subscriptions : ObservableCollection<Subscription>
    {
        public Subscriptions(ReaderAccount account)
            : base()
        {
            foreach (var subscription in account.Subscriptions)
            {
                Add(subscription);
            }
        }
    }
}
