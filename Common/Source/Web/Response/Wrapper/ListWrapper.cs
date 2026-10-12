using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Common.Source.Web.Response.Wrapper
{
    public class ListWrapper<T>
    {
        [JsonPropertyName("list")]
        public ImmutableArray<T> List { get; set; }
    }
}