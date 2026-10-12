using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class FullPostResponse : ResponseWrapper<PostWrapper<FullPostResponseWrapper>>, IContentSerializer<FullPostResponseWrapper?>
    {
        public FullPostResponseWrapper? Serialize()
        {
            return TryGetContent(out PostWrapper<FullPostResponseWrapper>? Content) ? Content.Post : default;
        }
    }
}