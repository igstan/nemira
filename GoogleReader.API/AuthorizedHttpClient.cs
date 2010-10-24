using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GoogleReader.API
{
    public class AuthorizedHttpClient : SimpleHttpClient
    {
        private const string LOGIN_URL = "https://www.google.com/accounts/ClientLogin";
        private const string TOKEN_URL = "https://www.google.com/reader/api/0/token";

        private string email;
        private string password;
        private string authToken;
        private string readerToken;

        public AuthorizedHttpClient(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public override string GET(string url, IDictionary<string, string> args, IDictionary<string, string> headers)
        {
            if (NotAuthorized()) Authorize();

            headers.Add("Authorization", String.Format("GoogleLogin auth={0}", authToken));
            return base.GET(url, args, headers);
        }

        public override string POST(string url, IDictionary<string, string> args, IDictionary<string, string> headers)
        {
            if (NotAuthorized()) Authorize();

            args.Add("T", readerToken);
            headers.Add("Authorization", String.Format("GoogleLogin auth={0}", authToken));

            return base.POST(url, args, headers);
        }

        private bool NotAuthorized()
        {
            return authToken == null && readerToken == null;
        }

        private void Authorize()
        {
            authToken = Authenticate(email, password);
            readerToken = AuthorizeWithReader(authToken);
        }

        private string Authenticate(string email, string password)
        {
            var response = base.POST(LOGIN_URL, new Dictionary<string, string>()
            {
                {"Email", email},
                {"Passwd", password},
                {"source", "cli-script"},
                {"service", "reader"},
            }, new Dictionary<string, string>() { });

            return response.Split('\n')[2].Split('=')[1];
        }

        private string AuthorizeWithReader(string authToken)
        {
            return GET(TOKEN_URL);
        }
    }
}
