using Common.Source.Core.Interface;
using Common.Source.Extension;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web.Response
{
    public class HttpResponse<TResult> : IExceptionCaptureOwner
    {
        public TResult? Body { get; }

        public HttpResponseHeaders? Headers { get; }

        public ExceptionDispatchInfo? CapturedException { get; set; }

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

        [MemberNotNull(nameof(Headers))]
        public void ThrowIfFailed()
        {
            this.ThrowIfExceptionCaptured();
            ArgumentNullException.ThrowIfNull(Headers);
        }

        public override string ToString()
        {
            return this.GetMessage(Body);
        }
    }
}