using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Builder
{
    public abstract class HoyolabHttpRequestMessageBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        public HoyolabToken HoyolabToken { get; set; }

        public HoyolabHttpRequestMessageBuilderFactory()
        {
            HoyolabToken = new HoyolabToken();
        }

        public HoyolabHttpRequestMessageBuilderFactory(HoyolabToken hoyolabToken)
        {
            HoyolabToken = hoyolabToken;
        }

        public abstract HttpRequestMessageBuilder Create();
    }
}