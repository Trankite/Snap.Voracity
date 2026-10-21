using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Interface;

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