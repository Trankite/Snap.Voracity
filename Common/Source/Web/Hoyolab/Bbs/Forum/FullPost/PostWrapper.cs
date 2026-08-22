using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class PostWrapper<T> where T : new()
    {
        [JsonPropertyName("post")]
        public T Post { get; set; } = new();
    }
}