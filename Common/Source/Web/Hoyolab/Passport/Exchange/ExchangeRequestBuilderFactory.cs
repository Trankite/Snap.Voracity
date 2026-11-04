using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Request.Builder.Abstract;
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
            ExchangeRequestBodySourceToken sourceToken = new(HoyolabToken.GetToken(Origin), (int)Origin);
            return new HoyolabHttpRequestMessageBuilder()
                .SetRequestUri(URL)
                .SetMethod(HttpMethod.Post)
                .SetStringContent(JsonSerializer.Serialize(new ExchangeRequestBody(sourceToken, HoyolabToken.Mid, (int)Destin)))
                .SetXrpcAppId(HoyolabOptions.HoyolabId);
        }
    }
}