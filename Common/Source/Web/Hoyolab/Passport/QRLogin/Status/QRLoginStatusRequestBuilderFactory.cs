using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using Common.Source.Web.Request.Builder.Abstraction;
using System.Text.Json;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin.Status
{
    public class QRLoginStatusRequestBuilderFactory : IHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://passport-api.mihoyo.com/account/ma-cn-passport/app/queryQRLoginStatus";

        public string Guid { get; set; } = string.Empty;

        public string Ticket { get; set; } = string.Empty;

        public QRLoginStatusRequestBuilderFactory() { }

        public HttpRequestMessageBuilder Create()
        {
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetStringContent(JsonSerializer.Serialize(new QRLoginStatusRequestBody(Ticket)))
                .SetXrpcAppId(HoyolabOptions.HoyolabId)
                .SetXrpcDeviceId(Guid);
        }
    }
}