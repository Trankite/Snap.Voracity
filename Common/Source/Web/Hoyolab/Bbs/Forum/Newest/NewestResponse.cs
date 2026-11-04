using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestResponse : ResponseWrapper<NewestResponseWrapper>, IContentSerializer<IEnumerable<NewestPost>?>
    {
        public IEnumerable<NewestPost>? Serialize()
        {
            return TryGetContent(out NewestResponseWrapper? newestPost) ? newestPost.List.Select(current => new NewestPost(current.Post.PostId, current.Post.Subject)) : default;
        }
    }
}