using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Avatar
{
    public class AvatarResponseServantDescIconMap
    {
        [JsonPropertyName("AvatarCyrene")]
        public string AvatarCyrene { get; set; } = string.Empty;
    }
}