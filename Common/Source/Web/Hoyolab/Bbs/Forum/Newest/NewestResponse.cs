using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public class NewestResponse : ResponseWrapper<NewestResponseWrapper>, IContentSerializer<IEnumerable<NewestAnalyzedBody>?>
    {
        public IEnumerable<NewestAnalyzedBody>? Serialize()
        {
            return TryGetContent(out NewestResponseWrapper? Content) ? Content.List.Select(Current => new NewestAnalyzedBody(Current.Post.PostId, Current.Post.Subject)) : default;
        }
    }
}