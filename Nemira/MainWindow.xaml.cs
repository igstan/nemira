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
            PopulateSubscriptionsTree(email, pass);
            Show();
        }

        private void PopulateSubscriptionsTree(string email, string pass)
        {
            subscriptions.ItemsSource = new Subscriptions(new ReaderAccount(email, pass));
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

                feedTitle.Text = subscription.Title;
                feedItemTooltip.Content = subscription.SourceUrl;
                sourceHyperlink.NavigateUri = new Uri(subscription.SourceUrl);
            }

            if (subscriptions.SelectedItem is SubscriptionItem)
            {
                var item = subscriptions.SelectedItem as SubscriptionItem;

                feedTitle.Text = item.Title;
                feedItemTooltip.Content = "Open in default Web browser";
                sourceHyperlink.NavigateUri = new Uri(item.SourceUrl);

                PopulateContentPane(item.Content);
            }
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
            //var addSubscriptionWindow = new AddSubscription();
            //addSubscriptionWindow.Owner = this;
            //addSubscriptionWindow.ShowDialog();

            //MessageBox.Show(addSubscriptionWindow.FeedUrl.Text);
            MessageBox.Show("Not Implemented");
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
