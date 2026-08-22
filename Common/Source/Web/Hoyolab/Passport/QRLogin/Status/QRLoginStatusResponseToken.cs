using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Passport.QRLogin.Status
{
    public class QRLoginStatusResponseToken
    {
        [JsonPropertyName("token_type")]
        public int TokenType { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}