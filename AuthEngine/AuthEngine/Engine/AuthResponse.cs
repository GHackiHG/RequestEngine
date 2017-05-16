using System.Collections.Generic;
using xNet;

namespace AuthEngine.Engine
{
    /// <summary>
    /// Содержит в себе результаты авторизации
    /// </summary>
    public class AuthResponse
    {
        public string Message { get; set; }
        public string Proxy { get; set; }
        public Response Response { get; set; }
        public Dictionary<string, string> RequestParams { get; set; } = new Dictionary<string, string>();
        public CookieDictionary Cookies = new CookieDictionary();
    }
}
