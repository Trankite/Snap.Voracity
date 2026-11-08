using Common.Source.Web.Request.Interface;
using System.Diagnostics;

namespace Common.Source.Web.Request.Abstract
{
    public static class HttpRequestMessageBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static HttpContext CreateHttpContext(this IHttpRequestMessageBuilderFactory builder, bool isHeadersRead = default, HttpClient? httpClient = default)
        {
            return builder.Create().CreateHttpContext(isHeadersRead, httpClient);
        }
    }
}