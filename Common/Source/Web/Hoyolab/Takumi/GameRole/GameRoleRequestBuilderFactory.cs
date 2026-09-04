using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Takumi.GameRole
{
    public class GameRoleRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://api-takumi.miyoushe.com/binding/api/getUserGameRolesByStoken";

        public GameRoleRequestBuilderFactory() { }

        public GameRoleRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetXrpcAppVersion(HoyolabOptions.Version)
                .SetXrpcClientType(HoyolabClient.Android)
                .SetDataSign(DataSignOptions.Create(SaltType.K2, DataSignAlgorithm.Gen1))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetMid().SetStoken());
        }
    }
}