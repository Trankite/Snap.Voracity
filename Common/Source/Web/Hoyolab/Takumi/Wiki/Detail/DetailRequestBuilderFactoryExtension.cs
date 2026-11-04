using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Detail
{
    public static class DetailRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static DetailRequestBuilderFactory SetContentId(this DetailRequestBuilderFactory builder, int value)
        {
            return builder.Configure(builder.ContentId = value);
        }
    }
}