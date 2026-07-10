using System.Text.Json.Serialization;

namespace StarRailDamage.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeRequestBodySourceToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public int TokenType { get; set; }

        public ExchangeRequestBodySourceToken(string token, int tokenType)
        {
            Token = token;
            TokenType = tokenType;
        }
    }
}