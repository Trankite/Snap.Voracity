using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.Takumi.Sign.Info;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Info
{
    public class SignInfoRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://api-takumi.mihoyo.com/event/luna/hkrpg/info";

        public HoyolabAction Action { get; set; }

        public HoyolabLanguage Language { get; set; }

        public string Server { get; set; } = string.Empty;

        public string Uid { get; set; } = string.Empty;

        public SignInfoRequestBuilderFactory() { }

        public SignInfoRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(new HoyolabHttpUriBuilder(URL).SetLanguage(Language).SetAction(Action).SetRegion(Server).SetUid(Uid))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetAccountMid().SetCookieToken());
        }
    }
}