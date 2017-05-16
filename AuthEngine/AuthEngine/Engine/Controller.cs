using System;
using xNet;
using System.Threading.Tasks;

namespace AuthEngine.Engine
{
    public class Controller : IAuthorizable
    {
        static Random rand = new Random();
        public AuthResponse Authorizate(AuthParams prms)
        {
            return new TaskFactory<AuthResponse>().StartNew(() =>
            {
                try
                {
                    using (var request = new HttpRequest())
                    {
                        request.IgnoreProtocolErrors = true;
                        request.ConnectTimeout = prms.TimeOut;
                        if (!string.IsNullOrWhiteSpace(prms.UserAgent))
                            request.UserAgent = prms.UserAgent;
                        foreach (var item in prms.RequestParams)
                        {
                            request.AddUrlParam(item.Key, item.Value);
                        }
                        if (prms.ProxyType != ProxyType.NONE)
                        {
                            if (!string.IsNullOrWhiteSpace(prms.Proxy))
                            {
                                if (prms.ProxyType == ProxyType.HTTPS)
                                    request.Proxy = HttpProxyClient.Parse(prms.Proxy);
                                if (prms.ProxyType == ProxyType.SOCKS4)
                                    request.Proxy = Socks4ProxyClient.Parse(prms.Proxy);
                                if (prms.ProxyType == ProxyType.SOCKS5)
                                    request.Proxy = Socks5ProxyClient.Parse(prms.Proxy);
                            }
                        }
                        HttpResponse response = null;
                        if (prms.RequestType == RequestType.GET)
                            response = request.Get(prms.RequestURI);
                        if (prms.RequestType == RequestType.POST)
                            response = request.Post(prms.RequestURI);
                        string result = response.ToString();
                        foreach (var item in prms.FilterCategory)
                        {
                            if (item.Value(result))
                            {
                                if (item.Key == "Captcha")
                                    return new AuthResponse { Message = "Капча.", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.Captcha, Cookies = response.Cookies };
                                else
                                if (item.Key == "Success")
                                    return new AuthResponse { Message = "Успешно.", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.GoodAccount, Cookies = response.Cookies };
                                else
                                if (item.Key == "Invalid")
                                    return new AuthResponse { Message = "Безуспешно.", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.BadAccount, Cookies = response.Cookies };
                                else
                                    return new AuthResponse { Message = item.Key, Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.CUSTOM, Cookies = response.Cookies };

                            }
                        }
                        if (string.IsNullOrWhiteSpace(result))
                            return new AuthResponse { Message = "Ошибка подключения.", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.ConnectionLost, Cookies = response.Cookies };
                        else
                            return new AuthResponse { Message = "Результат неопределен.", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.CUSTOM, Cookies = response.Cookies };
                    }
                }
                catch (Exception ex)
                {
                    return new AuthResponse { Message = $"При авторизации произошла ошибка: {ex.Message}", Proxy = prms.Proxy, RequestParams = prms.RequestParams, Response = Response.ConnectionLost };
                }
            }).Result;
        }
        public static string RandomUserAgent
        {
            get
            {
                var uas = Resources.ua.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                return uas[rand.Next(0,uas.Length-1)];
            }
        }
    }
}
