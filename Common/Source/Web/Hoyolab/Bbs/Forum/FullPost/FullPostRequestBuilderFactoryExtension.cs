using Common.Source.Extension;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.FullPost
{
    public static class FullPostRequestBuilderFactoryExtension
    {
        public static FullPostRequestBuilderFactory SetPostId(this FullPostRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.PostId = value);
        }
    }
}