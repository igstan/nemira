using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleReader.API
{
    public interface HttpClient
    {
        string GET(string url);
        string GET(string url, IDictionary<string, string> args);
        string POST(string url, IDictionary<string, string> args);
    }
}
