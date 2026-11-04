using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Interface;

namespace Common.Source.Web.EHentai.Builder
{
    public abstract class EHentaiHttpRequestMessageBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        public EHentaiToken? EHentaiToken { get; set; }

        public EHentaiHttpRequestMessageBuilderFactory() { }

        public EHentaiHttpRequestMessageBuilderFactory(EHentaiToken ehentaiToken)
        {
            EHentaiToken = ehentaiToken;
        }

        public abstract HttpRequestMessageBuilder Create();
    }
}