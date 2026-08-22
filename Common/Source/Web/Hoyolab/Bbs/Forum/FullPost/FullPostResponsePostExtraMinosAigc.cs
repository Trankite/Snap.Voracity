using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class FullPostResponsePostExtraMinosAigc
    {
        [JsonPropertyName("is_aigc")]
        public bool IsAigc { get; set; }
    }
}