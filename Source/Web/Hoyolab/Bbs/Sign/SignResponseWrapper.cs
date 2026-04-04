using System.Text.Json.Serialization;

namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Sign
{
    public class SignResponseWrapper
    {
        [JsonPropertyName("points")]
        public int Points { get; set; }
    }
}