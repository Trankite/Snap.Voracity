using Common.Source.Extension;
using System.Diagnostics;
using System.Net;

namespace Common.Source.Web.Request.Builder
{
    public static class HttpCookiesBuilderExtension
    {
        [DebuggerStepThrough]
        public static HttpCookiesBuilder AddCookie(this HttpCookiesBuilder builder, string name, string? value)
        {
            return builder.AddCookie(new Cookie(name, value));
        }

        [DebuggerStepThrough]
        public static HttpCookiesBuilder AddCookie(this HttpCookiesBuilder builder, Cookie cookie)
        {
            return builder.Configure(builder => builder.Collection.Add(cookie));
        }

        [DebuggerStepThrough]
        public static HttpCookiesBuilder SetCookie(this HttpCookiesBuilder builder, string name, string? value)
        {
            return builder.SetCookie(new Cookie(name, value));
        }

        [DebuggerStepThrough]
        public static HttpCookiesBuilder SetCookie(this HttpCookiesBuilder builder, Cookie cookie)
        {
            return builder.Configure(builder.RemoveCookie(cookie).AddCookie(cookie));
        }

        [DebuggerStepThrough]
        public static HttpCookiesBuilder RemoveCookie(this HttpCookiesBuilder builder, string name, string? value)
        {
            return builder.RemoveCookie(new Cookie(name, value));
        }

        [DebuggerStepThrough]
        public static HttpCookiesBuilder RemoveCookie(this HttpCookiesBuilder builder, Cookie cookie)
        {
            return builder.Configure(builder.Collection.Remove(cookie));
        }
    }
}