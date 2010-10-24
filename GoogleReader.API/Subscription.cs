using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleReader.API
{
    public class Subscription
    {
        private ReaderAccount googleReader;

        public Subscription(ReaderAccount googleReader)
        {
            this.googleReader = googleReader;
        }

        public IEnumerable<SubscriptionItem> Items
        {
            get { return googleReader.ItemsForSubscription(this); }
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string SourceUrl { get; set; }
    }
}
