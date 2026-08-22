using Common.Source.Extension;
using Common.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public class FullPostResponse : ResponseWrapper<PostWrapper<FullPostResponseWrapper>, FullPostResponseWrapper>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out FullPostResponseWrapper? analyedBody)
        {
            return TryGetAnalyzedBody(out PostWrapper<FullPostResponseWrapper>? Content) ? true.Configure(analyedBody = Content.Post) : false.Configure(analyedBody = default);
        }
    }
}