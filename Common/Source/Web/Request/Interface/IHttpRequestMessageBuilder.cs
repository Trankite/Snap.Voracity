using Common.Source.Web.Request.Builder.Interface;
using System.Net.Http.Headers;

namespace Common.Source.Web.Request.Interface
{
    public interface IHttpRequestMessageBuilder<out T> : IHttpRequestUriBuilder, IHttpMethodBuilder, IHttpRequestOptionsBuilder, IHttpHeadersBuilder<T>, IHttpContentBuilder where T : HttpHeaders
    {
        HttpRequestMessage HttpRequestMessage { get; }
    }
}