using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public static class FullPostRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static FullPostRequestBuilderFactory SetPostId(this FullPostRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.PostId = value);
        }
    }
}