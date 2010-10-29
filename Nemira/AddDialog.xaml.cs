using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Nemira
{
    public partial class AddDialog : Window
    {
        public AddDialog()
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
