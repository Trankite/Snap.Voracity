using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using System.Text.Json;

namespace Common.Source.Web.Hoyolab.Bbs.Sign
{
    public class SignRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://bbs-api.miyoushe.com/apihub/app/api/signIn";

        public HoyolabGroup Group { get; set; }

        public SignRequestBuilderFactory() { }

        public SignRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetReferer(HoyolabReferer.MihoyoApp)
                .SetXrpcAppVersion(HoyolabOptions.Version)
                .SetXrpcClientType(HoyolabClient.Android)
                .SetDataSignWithBody(DataSignOptions.Create(SaltType.X6, DataSignAlgorithm.Gen2), JsonSerializer.Serialize(new SignRequestBody(Group.GetIntString())))
                .SetHeader(new HoyolabCookieBuilder(HoyolabToken).SetMid().SetStoken());
        }
    }
}