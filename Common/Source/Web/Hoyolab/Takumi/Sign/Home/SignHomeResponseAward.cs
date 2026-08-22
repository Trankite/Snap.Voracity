using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public class SignHomeResponseAward
    {
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("cnt")]
        public int Count { get; set; }
    }
}