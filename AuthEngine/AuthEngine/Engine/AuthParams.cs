using System;
using System.Collections.Generic;

namespace AuthEngine.Engine
{
    public class AuthParams
    {
        public AuthParams() { }
        public AuthParams(Uri _requestUri, Dictionary<string, string> _requestParams, string _proxy = null, RequestType _requestType = RequestType.GET, ProxyType _proxyType = ProxyType.NONE)
        {
            RequestURI = _requestUri;
            RequestParams = _requestParams;
            RequestType = _requestType;
            ProxyType = _proxyType;
            Proxy = _proxy;
        }
        public RequestType RequestType { get; set; }
        public ProxyType ProxyType { get; set; }
        public Uri RequestURI { get; set; }
        public int TimeOut { get; set; }
        public String Proxy { get; set; } = string.Empty;
        public Dictionary<string, string> RequestParams { get; set; } = new Dictionary<string, string>();
        public string UserAgent { get; set; } = string.Empty;
        public Dictionary<string, Func<string, bool>> FilterCategory { get; set; } = new Dictionary<string, Func<string, bool>>();
    }
}
