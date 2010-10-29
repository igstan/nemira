using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using GoogleReader.API;

namespace Nemira
{
    public partial class MainWindow : Window
    {
        private ReaderAccount readerAccount;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void OpenAccount(string email, string pass)
        {
            this.readerAccount = new ReaderAccount(email, pass);

            LoadSubscriptions();
            Show();
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

        private void OnSelectedFeed(object sender, RoutedEventArgs e)
        {
            if (subscriptions.SelectedItem is Subscription)
            {
                var subscription = subscriptions.SelectedItem as Subscription;

                EnableSubscriptionToolbarButtons();

                feedTitle.Text = subscription.Title;
                feedItemTooltip.Content = subscription.SourceUrl;
                sourceHyperlink.NavigateUri = new Uri(subscription.SourceUrl);
            }

            if (subscriptions.SelectedItem is SubscriptionItem)
            {
                var item = subscriptions.SelectedItem as SubscriptionItem;

                DisableSubscriptionToolbarButtons();

                feedTitle.Text = item.Title;
                feedItemTooltip.Content = "Open in default Web browser";
                sourceHyperlink.NavigateUri = new Uri(item.SourceUrl);

                PopulateContentPane(item.Content);
            }
        }

        private void OnUnselectedFeed(object sender, RoutedEventArgs e)
        {
            DisableSubscriptionToolbarButtons();
        }

        private void EnableSubscriptionToolbarButtons()
        {
            editSubscription.IsEnabled = true;
            deleteSubscription.IsEnabled = true;
        }

        private void DisableSubscriptionToolbarButtons()
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

        private void AddSubscription(object sender, ExecutedRoutedEventArgs e)
        {
            var addSubscriptionDialog = new AddSubscription();
            addSubscriptionDialog.Owner = this;

            if (addSubscriptionDialog.ShowDialog() == true)
            {
                readerAccount.AddSubscription(addSubscriptionDialog.FeedUrl);
                LoadSubscriptions();
            }
        }

        private void RemoveSubscription(object sender, ExecutedRoutedEventArgs e)
        {
            var subscription = subscriptions.SelectedItem as Subscription;
            var message = String.Format("Unsubscribe from {0}?", subscription.Title);
            var title = "Confirm Unsubscribe";
            var button = MessageBoxButton.OKCancel;
            var icon = MessageBoxImage.Warning;

            if (MessageBox.Show(message, title, button, icon) == MessageBoxResult.OK) {
                readerAccount.RemoveSubscription(subscription);
                LoadSubscriptions();
            }
        }

        private void CanRemoveSubscription(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = subscriptions != null
                        && subscriptions.SelectedItem is Subscription;
        }

        private void OnRenameSubscription(object sender, RoutedEventArgs e)
        {
            var subscription = subscriptions.SelectedItem as Subscription;
            var renameDialog = new RenameSubscription();
            renameDialog.Owner = this;
            renameDialog.SubscriptionName = subscription.Title;

            if (renameDialog.ShowDialog() == true)
            {
                readerAccount.RenameSubscription(subscription, renameDialog.SubscriptionName);
                LoadSubscriptions();
            }
        }

        private void LoadSubscriptions()
        {
            subscriptions.ItemsSource = new ObservableCollection<Subscription>(readerAccount.Subscriptions);
        }
    }
}
