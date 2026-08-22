using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Avatar
{
    public class AvatarResponseAvatarRelicProperty
    {
        [JsonPropertyName("property_type")]
        public int PropertyType { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("times")]
        public int Times { get; set; }
    }
}