using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeRequestBody
    {
        [JsonPropertyName("src_token")]
        public ExchangeRequestBodySourceToken SourceToken { get; set; }

        [JsonPropertyName("mid")]
        public string Mid { get; set; } = string.Empty;

        [JsonPropertyName("dst_token_type")]
        public int DestinationTokenType { get; set; }

        public ExchangeRequestBody(ExchangeRequestBodySourceToken sourceToken, string mid, int destinationTokenType)
        {
            SourceToken = sourceToken;
            Mid = mid;
            DestinationTokenType = destinationTokenType;
        }
    }
}