using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GoogleReader.API;

namespace Nemira
{
    class SubscriptionCollection : ObservableCollection<Subscription>
    {
        public SubscriptionCollection(ReaderAccount account) : base()
        {
            foreach (var subscription in account.Subscriptions)
            {
                Add(subscription);
            }
        }
    }
}
