using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Interface;

namespace Common.Source.Web.EHentai.Builder
{
    public class EHentaiHttpRequestMessageBuilder : HttpRequestMessageBuilder
    {
        public EHentaiHttpRequestMessageBuilder() { }

        public EHentaiHttpRequestMessageBuilder(IHttpContentSerializer httpContentSerializer) : base(httpContentSerializer) { }

        public EHentaiHttpRequestMessageBuilder(IHttpContentSerializer httpContentSerializer, HttpRequestMessage httpRequestMessage) : base(httpContentSerializer, httpRequestMessage) { }
    }
}