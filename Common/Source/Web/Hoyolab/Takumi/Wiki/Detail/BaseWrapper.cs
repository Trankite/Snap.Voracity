using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class BaseWrapper<T>
    {
        [JsonPropertyName("base")]
        public T? Base { get; set; }
    }
}