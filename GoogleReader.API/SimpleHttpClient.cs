using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace GoogleReader.API
{
    public class SimpleHttpClient : HttpClient
    {
        public string GET(string url)
        {
            return GET(url, new Dictionary<string, string>());
        }

        public string GET(string url, IDictionary<string, string> args)
        {
            return GET(url, args, new Dictionary<string, string>());
        }

        public virtual string GET(string url, IDictionary<string, string> args, IDictionary<string, string> headers)
        {
            var request = WebRequest.Create(url + "?" + BuildQueryString(args));

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return FetchResponseString(request);
        }

        public string POST(string url)
        {
            return POST(url, new Dictionary<string, string>());
        }

        public string POST(string url, IDictionary<string, string> args)
        {
            return POST(url, args, new Dictionary<string, string>());
        }

        public virtual string POST(string url, IDictionary<string, string> args, IDictionary<string, string> headers)
        {
            var queryString = BuildQueryString(args);
            var request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = queryString.Length;

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            using (var requestStream = request.GetRequestStream())
            {
                var bytes = new UTF8Encoding().GetBytes(queryString);
                requestStream.Write(bytes, 0, bytes.Length);
            }

            return FetchResponseString(request);
        }

        private static string FetchResponseString(WebRequest request)
        {
            using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                        return reader.ReadToEnd();
        }

        private static string BuildQueryString(IDictionary<string, string> args)
        {
            if (args.Count == 0) return string.Empty;

            return args.Keys.Zip(args.Values, (key, value) => key + "=" + value)
                            .Aggregate((previous, current) => previous + "&" + current);
        }
    }
}
