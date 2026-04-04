using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using StarRailDamage.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestResponse : ResponseWrapper<NewestResponseWrapper, NewestAnalyzedBody[]>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out NewestAnalyzedBody[]? analyedBody)
        {
            if (TryGetAnalyzedBody(out NewestResponseWrapper? Content))
            {
                analyedBody = new NewestAnalyzedBody[Content.List.Length];
                for (int i = 0; i < analyedBody.Length; i++)
                {
                    FullPostResponsePost Post = Content.List[i].Post;
                    analyedBody[i] = new NewestAnalyzedBody(Post.PostId, Post.Subject);
                }
                return true;
            }
            return false.Configure(analyedBody = default);
        }
    }
}