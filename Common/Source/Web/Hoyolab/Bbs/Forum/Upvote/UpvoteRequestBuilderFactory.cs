using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using System.Text.Json;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Upvote
{
    public class UpvoteRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/post/api/post/upvote";

        public bool IsCancel { get; set; }

        public string PostId { get; set; } = string.Empty;

        public UpvoteRequestBuilderFactory() { }

        public UpvoteRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetStringContent(JsonSerializer.Serialize(UpvoteRequestBody.Create(PostSource.Discussion, IsCancel, PostId, BoolExtension.ToIntString(!IsCancel))))
                .SetReferer(HoyolabReferer.MihoyoApp)
                .SetXrpcAppVersion(HoyolabOptions.Version)
                .SetXrpcClientType(HoyolabClient.Android)
                .SetDataSign(DataSignOptions.Create(SaltType.K2, DataSignAlgorithm.Gen1))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetMid().SetStoken());
        }
    }
}