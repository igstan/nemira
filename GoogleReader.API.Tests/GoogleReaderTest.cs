using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GoogleReader.API;

namespace GoogleReader.API.Tests
{
    [TestFixture]
    public class GoogleReaderTest
    {
        private ReaderUrls endpointUrls = new ReaderUrls()
        {
            SubscriptionList = "subscriptions-url",
            SubscriptionItems = "subscription-items-url",
            AddSubscription = "add-subscription",
        };

        [Test]
        public void CanFetchSubscriptionList()
        {
            var httpClient = new StubHttpClient()
            {
                ExpectedUrl = endpointUrls.SubscriptionList,
                ExpectedResponse = @"{
                    ""subscriptions"": [
                        {
                            ""id"": ""feed/http://feeds.feedburner.com/ajaxian"",
                            ""title"": ""Ajaxian » Front Page""
                        }
                    ]
                }",
            };

            var reader = new ReaderAccount(httpClient, endpointUrls);

            Assert.AreEqual("feed/http://feeds.feedburner.com/ajaxian", reader.Subscriptions.ElementAt(0).Id);
            Assert.AreEqual("Ajaxian » Front Page", reader.Subscriptions.ElementAt(0).Title);
        }

        [Test]
        public void CanParseAtomBasedSubscriptionItems()
        {
            var httpClient = new StubHttpClient()
            {
                ExpectedUrl = endpointUrls.SubscriptionItems + "feed%2Fid", // feed ID must be URL-encoded
                ExpectedResponse = @"{
                    ""items"": [
                        {
                            ""title"": ""Ajaxian » Front Page"",
                            ""content"": {
                                ""content"": ""Dummy Content""
                            }
                        }
                    ]
                }",
            };

            var reader = new ReaderAccount(httpClient, endpointUrls);
            var subscription = new Subscription(null)
            {
                Id = "feed/id",
            };

            Assert.AreEqual("Ajaxian » Front Page", reader.ItemsForSubscription(subscription).ElementAt(0).Title);
            Assert.AreEqual("Dummy Content", reader.ItemsForSubscription(subscription).ElementAt(0).Content);
        }

        [Test]
        public void CanParseRssBasedSubscriptionItems()
        {
            var httpClient = new StubHttpClient()
            {
                ExpectedUrl = endpointUrls.SubscriptionItems + "feed%2Fid", // feed ID must be URL-encoded
                ExpectedResponse = @"{
                    ""items"": [
                        {
                            ""title"": ""Ajaxian » Front Page"",
                            ""summary"": {
                                ""content"": ""Dummy Content""
                            }
                        }
                    ]
                }",
            };

            var reader = new ReaderAccount(httpClient, endpointUrls);
            var subscription = new Subscription(null)
            {
                Id = "feed/id",
            };

            Assert.AreEqual("Ajaxian » Front Page", reader.ItemsForSubscription(subscription).ElementAt(0).Title);
            Assert.AreEqual("Dummy Content", reader.ItemsForSubscription(subscription).ElementAt(0).Content);
        }

        [Test]
        public void CanAddNewSubscription()
        {
            var httpClient = new StubHttpClient()
            {
                ExpectedUrl = endpointUrls.AddSubscription,
                ExpectedResponse = "",
            };

            var reader = new ReaderAccount(httpClient, endpointUrls);
            reader.AddSubscription("http://feeds.feedburner.com/ajaxian");
        }

        class StubHttpClient : HttpClient
        {
            public string ExpectedUrl { get; set; }

            public string ExpectedResponse { get; set; }

            public string GET(string url)
            {
                Assert.AreEqual(ExpectedUrl, url);
                return ExpectedResponse;
            }

            public string GET(string url, IDictionary<string, string> args)
            {
                Assert.AreEqual(ExpectedUrl, url);
                return ExpectedResponse;
            }

            public string POST(string url, IDictionary<string, string> args)
            {
                Assert.AreEqual(ExpectedUrl, url);
                return ExpectedResponse;
            }
        }
    }
}
