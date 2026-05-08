namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestAnalyzedBody
    {
        public string PostId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public NewestAnalyzedBody() { }

        public NewestAnalyzedBody(string postId, string title)
        {
            PostId = postId;
            Title = title;
        }
    }
}