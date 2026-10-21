using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;

namespace Common.Source.Web.Hoyolab.Bbs.Mission
{
    public class MissionRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/apihub/wapi/getUserMissionsState";

        public MissionRequestBuilderFactory() { }

        public MissionRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder().SetRequestUri(URL).SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetLtuid().SetLtoken());
        }
    }
}