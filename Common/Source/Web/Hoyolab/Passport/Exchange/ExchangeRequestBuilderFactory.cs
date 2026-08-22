using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Request.Builder;
using System.Text.Json;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeRequestBuilderFactory : HoyolabHttpRequestMessageBuilderFactory
    {
        private const string URL = "https://passport-api.mihoyo.com/account/ma-cn-session/app/exchange";

        public HoyolabTokenType Origin { get; set; }

        public HoyolabTokenType Destin { get; set; }

        public ExchangeRequestBuilderFactory() { }

        public ExchangeRequestBuilderFactory(HoyolabToken hoyolabToken) : base(hoyolabToken) { }

        public override HttpRequestMessageBuilder Create()
        {
            ExchangeRequestBodySourceToken SourceToken = new(HoyolabToken.GetToken(Origin), Origin.ToInt());
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetStringContent(JsonSerializer.Serialize(new ExchangeRequestBody(SourceToken, HoyolabToken.Mid, Destin.ToInt())))
                .SetXrpcAppId(HoyolabOptions.HoyolabId);
        }
    }
}