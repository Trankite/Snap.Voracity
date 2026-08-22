using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeResponseToken
    {
        [JsonPropertyName("token_type")]
        public int TokenType { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}