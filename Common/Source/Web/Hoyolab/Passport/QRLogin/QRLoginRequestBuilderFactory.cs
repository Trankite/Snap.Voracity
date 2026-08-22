using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin
{
    public class QRLoginRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        private static readonly string URL = "https://passport-api.mihoyo.com/account/ma-cn-passport/app/createQRLogin";

        public string Guid { get; set; } = string.Empty;

        public QRLoginRequestBuilderFactory() { }

        public HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetXrpcAppId(HoyolabOptions.HoyolabId)
                .SetXrpcDeviceId(Guid);
        }
    }
}