using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://api-takumi.mihoyo.com/event/luna/hkrpg/home";

        public HoyolabAction Action { get; set; }

        public HoyolabLanguage Language { get; set; }

        public SignHomeRequestBuilderFactory() { }

        public SignHomeRequestBuilderFactory(HoyolabLanguage language, HoyolabAction hoyolabAction)
        {
            Language = language;
            Action = hoyolabAction;
        }

        public HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder().SetRequestUri(new HoyolabHttpUriBuilder(URL).SetLanguage(Language).SetAction(Action));
        }
    }
}