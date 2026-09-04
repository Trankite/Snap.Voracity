using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.Metadata;
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

        public HoyolabUserRole UserRole { get; set; } = new();

        public SignInfoRequestBuilderFactory() { }

        public SignInfoRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public SignInfoRequestBuilderFactory(HoyolabToken hoyolabToken, HoyolabAction action, HoyolabLanguage language, HoyolabUserRole userRole) : base(hoyolabToken)
        {
            Action = action;
            Language = language;
            UserRole = userRole;
        }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(new HoyolabHttpUriBuilder(URL).SetLanguage(Language).SetAction(Action).SetRegion(UserRole.Server).SetUid(UserRole.Uid))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetAccountMid().SetCookieToken());
        }
    }
}