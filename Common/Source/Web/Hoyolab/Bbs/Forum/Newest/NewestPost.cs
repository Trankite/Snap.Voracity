namespace Common.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestPost
    {
        public string Title { get; set; } = string.Empty;

        public string PostId { get; set; } = string.Empty;

        public NewestPost() { }

        public NewestPost(string postId, string title)
        {
            PostId = postId;
            Title = title;
        }
    }
}