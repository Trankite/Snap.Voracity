using System.Net.Http.Headers;

namespace Common.Source.Web.Request.Builder.Abstraction
{
    public interface IHttpHeadersBuilder<out T> where T : HttpHeaders
    {
        T Headers { get; }
    }
}