using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class DetailResponseMetaContent
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}