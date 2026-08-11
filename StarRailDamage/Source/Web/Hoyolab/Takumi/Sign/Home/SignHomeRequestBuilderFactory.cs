using StarRailDamage.Source.Web.Hoyolab.Builder;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Request.Builder;
using StarRailDamage.Source.Web.Request.Builder.Abstraction;

namespace StarRailDamage.Source.Web.Hoyolab.Takumi.Sign.Home
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
            return new HoyolabHttpRequestMessageBuilder().SetRequestUri(new HoyolabHttpUriBuilder(URL).SetLanguage(Language).SetAction(Action).Uri);
        }
    }
}