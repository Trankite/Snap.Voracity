using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class FullPostResponseRecommend
    {
        [JsonPropertyName("tags")]
        public ImmutableArray<FullPostResponseRecommendTag> Tags { get; set; }

        [JsonPropertyName("lottery")]
        public FullPostResponseRecommendLottery Lottery { get; set; } = new();

        [JsonPropertyName("is_following")]
        public bool IsFollowing { get; set; }

        [JsonPropertyName("is_mentor_rec_block")]
        public bool IsMentorRecBlock { get; set; }
    }
}