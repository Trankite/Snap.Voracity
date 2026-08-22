using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class DetailResponseChannelSlice
    {
        [JsonPropertyName("channel_id")]
        public int ChannelId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("ch_ext")]
        public string ChannelExtension { get; set; } = string.Empty;

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }
    }
}