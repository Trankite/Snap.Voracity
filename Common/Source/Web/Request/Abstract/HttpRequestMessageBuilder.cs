using Common.Source.Web.Request.Interface;
using System.Net.Http.Headers;

namespace Common.Source.Web.Request.Abstract
{
    public class HttpRequestMessageBuilder : IHttpRequestMessageBuilder<HttpHeaders>, IDisposable
    {
        private bool Disposed;

        public Uri? RequestUri
        {
            get => HttpRequestMessage.RequestUri;
            set => HttpRequestMessage.RequestUri = value;
        }

        public HttpMethod Method
        {
            get => HttpRequestMessage.Method;
            set => HttpRequestMessage.Method = value;
        }

        public HttpRequestOptions Options => HttpRequestMessage.Options;

        public HttpHeaders Headers => HttpRequestMessage.Headers;

        public HttpContent? Content
        {
            get => HttpRequestMessage.Content;
            set => HttpRequestMessage.Content = value;
        }

        public HttpRequestMessage HttpRequestMessage { get; set; } = new();

        public IHttpContentSerializer HttpContentSerializer { get; }

        public HttpRequestMessageBuilder()
        {
            HttpContentSerializer = new JsonHttpContentSerializer();
        }

        public HttpRequestMessageBuilder(IHttpContentSerializer httpContentSerializer)
        {
            HttpContentSerializer = httpContentSerializer;
        }

        public HttpRequestMessageBuilder(IHttpContentSerializer httpContentSerializer, HttpRequestMessage httpRequestMessage) : this(httpContentSerializer)
        {
            HttpRequestMessage = httpRequestMessage;
        }

        public override string ToString()
        {
            return HttpRequestMessage.ToString();
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                HttpRequestMessage.Dispose();
                GC.SuppressFinalize(this);
                Disposed = true;
            }
        }
    }
}