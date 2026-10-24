using Common.Source.Extension;
using Common.Source.Model.Interface;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web.Response
{
    public class HttpResponse<TResult> : IExceptionable
    {
        public TResult? Body { get; }

        public HttpResponseHeaders? Headers { get; }

        public ExceptionDispatchInfo? CapturedException { get; }

        public HttpResponse() { }

        public HttpResponse(HttpResponseHeaders? headers)
        {
            Headers = headers;
        }

        public HttpResponse(HttpResponseHeaders? headers, TResult? body) : this(headers)
        {
            Body = body;
        }

        public HttpResponse(HttpResponseHeaders? headers, ExceptionDispatchInfo? capturedException) : this(headers)
        {
            CapturedException = capturedException;
        }

        public override string ToString()
        {
            return CapturedException.GetExceptionMessage(Body);
        }
    }
}