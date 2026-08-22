using System.Net;

namespace Common.Source.Web.Request.Builder
{
    public class HttpCookiesBuilder
    {
        public CookieCollection Collection { get; } = [];

        public override string ToString()
        {
            return string.Join(';', Collection.Select(Cookie => $"{Cookie.Name}={Cookie.Value}"));
        }
    }
}