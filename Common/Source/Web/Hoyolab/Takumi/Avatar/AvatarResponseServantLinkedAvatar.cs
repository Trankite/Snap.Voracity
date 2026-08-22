using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Avatar
{
    public class AvatarResponseServantLinkedAvatar
    {
        [JsonPropertyName("avatar_id")]
        public string AvatarId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}