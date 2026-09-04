using Common.Source.Web.Hoyolab.Bbs.Forum.Share;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Share
{
    public class ShareRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/apihub/api/getShareConf";

        public EntityType EntityType { get; set; }

        public string EntityId { get; set; } = string.Empty;

        public ShareRequestBuilderFactory() { }

        public ShareRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(new HoyolabHttpUriBuilder(URL).SetEntityType(EntityType).SetEntityId(EntityId))
                .SetReferer(HoyolabReferer.MihoyoApp)
                .SetXrpcAppVersion(HoyolabOptions.Version)
                .SetXrpcClientType(HoyolabClient.Android)
                .SetDataSign(DataSignOptions.Create(SaltType.K2, DataSignAlgorithm.Gen1))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetMid().SetStoken());
        }
    }
}