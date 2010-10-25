using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GoogleReader.API
{
    public partial class ReaderAccount
    {
        private HttpClient httpClient;
        private ReaderUrls urls;

        public ReaderAccount(string email, string password)
        {
            this.httpClient = new AuthorizedHttpClient(email, password);
            this.urls = new ReaderUrls();
        }

        public ReaderAccount(HttpClient httpClient, ReaderUrls urls)
        {
            this.httpClient = httpClient;
            this.urls = urls;
        }

        public IEnumerable<Subscription> Subscriptions
        {
            get
            {
                var args = new Dictionary<string, string>()
                {
                    {"output", "json"},
                };
                var json = httpClient.GET(urls.SubscriptionList, args);
                var serializer = new DataContractJsonSerializer(typeof(JsonSubscriptions));

                using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var container = serializer.ReadObject(jsonStream) as JsonSubscriptions;
                    return container.subscriptions.Select(subscription => new Subscription(this)
                        {
                            Id = subscription.id,
                            Title = subscription.title,
                        });
                }
            }

            private set { }
        }

        public IEnumerable<SubscriptionItem> ItemsForSubscription(Subscription subscription)
        {
            var args = new Dictionary<string, string>()
            {
                {"n", "20"},
            };

            var subscriptionId = System.Uri.EscapeDataString(subscription.Id);

            var json = httpClient.GET(urls.SubscriptionItems + subscriptionId, args);
            var serializer = new DataContractJsonSerializer(typeof(JsonSubscriptionItems));

            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var container = serializer.ReadObject(jsonStream) as JsonSubscriptionItems;

                try
                {
                    subscription.SourceUrl = container.alternate.ElementAt(0).href;
                }
                catch (Exception e)
                {
                    subscription.SourceUrl = "http://www.google.com/reader/";
                }

                return container.items.Select(CreateSubscriptionItem);
            }
        }

        private SubscriptionItem CreateSubscriptionItem(JsonSubscriptionItem item)
        {
            String title = "Missing title";
            String content = "Missing content";
            String sourceUrl = "Missing source URL";

            try { title = item.title; }
            catch (Exception ignore) { }

            try { content = item.content != null ? item.content.content : item.summary.content; }
            catch (Exception ignore) { }

            try { sourceUrl = item.alternate.ElementAt(0).href; }
            catch (Exception ignore) { }

            return new SubscriptionItem()
            {
                Title = title,
                Content = content,
                SourceUrl = sourceUrl,
            };
        }

        public void AddSubscription(string subscriptionUrl)
        {
            var args = new Dictionary<string, string>()
            {
                {"quickadd", subscriptionUrl},
            };

            httpClient.POST(urls.AddSubscription, args);
        }

        public void RemoveSubscription(Subscription subscription)
        {
            var args = new Dictionary<string, string>()
            {
                {"ac", "unsubscribe"},
                {"s", subscription.Id},
            };

            httpClient.POST(urls.EditSubscription, args);
        }

        public void RenameSubscription(Subscription subscription, string newName)
        {
            var args = new Dictionary<string, string>()
            {
                {"ac", "edit"},
                {"s", subscription.Id},
                {"t", newName}
            };

            httpClient.POST(urls.EditSubscription, args);
        }
    }
}
