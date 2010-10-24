using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nemira
{
    public partial class AddSubscription : Window
    {
        public AddSubscription()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(OnLoaded);
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            feedUrl.Text = Clipboard.GetText();
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string FeedUrl
        {
            get
            {
                return feedUrl.Text;
            }
        }
    }
}
