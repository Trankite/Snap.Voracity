using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Upvote
{
    public static class UpvoteRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static UpvoteRequestBuilderFactory SetIsCancel(this UpvoteRequestBuilderFactory builder, bool value)
        {
            return builder.Configure(builder.IsCancel = value);
        }

        [DebuggerStepThrough]
        public static UpvoteRequestBuilderFactory SetPostId(this UpvoteRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.PostId = value);
        }
    }
}