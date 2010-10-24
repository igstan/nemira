using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GoogleReader.API
{
    [DataContract]
    class JsonSubscriptions
    {
        [DataMember]
        public IList<JsonSubscription> subscriptions;
    }

    [DataContract]
    class JsonSubscription
    {
        [DataMember]
        public string id;
        [DataMember]
        public string title;
    }

    [DataContract]
    class JsonSubscriptionItems
    {
        [DataMember]
        public IList<JsonSubscriptionItem> items;
        [DataMember]
        public IList<JsonAlternate> alternate;
    }

    [DataContract]
    class JsonAlternate
    {
        [DataMember]
        public string href;
        [DataMember]
        public string type;
    }

    [DataContract]
    class JsonSubscriptionItem
    {
        [DataMember]
        public string title;
        [DataMember]
        public JsonSubscriptionItemContent content;
        [DataMember]
        public JsonSubscriptionItemContent summary;
        [DataMember]
        public IList<JsonAlternate> alternate;
    }

    [DataContract]
    class JsonSubscriptionItemContent
    {
        [DataMember]
        public string content;
    }
}
