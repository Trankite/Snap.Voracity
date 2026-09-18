using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.EHentai.Builder
{
    public abstract class EHentaiHttpRequestMessageBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        public EHentaiToken? EHentaiToken { get; set; }

        public EHentaiHttpRequestMessageBuilderFactory() { }

        public EHentaiHttpRequestMessageBuilderFactory(EHentaiToken eHentaiToken)
        {
            EHentaiToken = eHentaiToken;
        }

        public abstract HttpRequestMessageBuilder Create();
    }
}