using Common.Source.Extension;
using Common.Source.Model.Interface;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;

namespace Common.Source.Web.Response
{
    public class HttpResponse<TResult> : IExceptionable, IExceptionThrowable
    {
        public TResult? Body { get; }

        public HttpResponseHeaders? Headers { get; }

        public ExceptionDispatchInfo? CapturedException { get; }

        [MemberNotNullWhen(true, nameof(Body), nameof(Headers))]
        public bool IsReady => CapturedException.IsNull();

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

        [StackTraceHidden]
        [MemberNotNull(nameof(Body), nameof(Headers))]
        public void ThrowIfFailed()
        {
            CapturedException?.Throw();
            ArgumentNullException.ThrowIfNull(Body);
            ArgumentNullException.ThrowIfNull(Headers);
        }

        public override string ToString()
        {
            return CapturedException.GetExceptionMessage(Body);
        }
    }
}