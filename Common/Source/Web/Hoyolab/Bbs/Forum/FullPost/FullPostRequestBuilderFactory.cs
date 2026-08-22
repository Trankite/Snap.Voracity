using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class FullPostRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/post/api/getPostFull";

        public string PostId { get; set; } = string.Empty;

        public FullPostRequestBuilderFactory() { }

        public FullPostRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(new HoyolabHttpUriBuilder(URL).SetPostId(PostId))
                .SetReferer(HoyolabReferer.MihoyoApp)
                .SetXrpcAppVersion(HoyolabOptions.Version)
                .SetXrpcClientType(HoyolabClient.Android)
                .SetDataSign(DataSignOptions.Create(SaltType.K2, DataSignAlgorithm.Gen1))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetMid().SetStoken());
        }
    }
}