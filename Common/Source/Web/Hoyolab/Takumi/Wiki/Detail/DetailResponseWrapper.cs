using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public class DetailResponseWrapper
    {
        [JsonPropertyName("content")]
        public DetailResponseContent? Content { get; set; }

        [JsonPropertyName("channel_list")]
        public ImmutableArray<DetailResponseChannel> ChannelList { get; set; }
    }
}