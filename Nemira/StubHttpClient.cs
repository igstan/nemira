using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleReader.API;

namespace Nemira
{
    class StubHttpClient : SimpleHttpClient
    {
        public override string GET(string url, IDictionary<string, string> args, IDictionary<string, string> headers)
        {
            url = url.Contains("subscription")
                ? "http://macbook/reader/list.json"
                : "http://macbook/reader/feed-contents.json";

            return base.GET(url, args, headers);
        }
    }
}
