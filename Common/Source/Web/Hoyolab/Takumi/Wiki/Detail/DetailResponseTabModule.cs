using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class DetailResponseTabModule
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pos")]
        public string Pos { get; set; } = string.Empty;
    }
}