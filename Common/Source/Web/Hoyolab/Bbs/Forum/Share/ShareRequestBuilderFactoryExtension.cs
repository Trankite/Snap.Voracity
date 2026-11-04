using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Share
{
    public static class ShareRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static ShareRequestBuilderFactory SetEntityType(this ShareRequestBuilderFactory builder, EntityType value)
        {
            return builder.Configure(builder.EntityType = value);
        }

        [DebuggerStepThrough]
        public static ShareRequestBuilderFactory SetEntityId(this ShareRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.EntityId = value);
        }
    }
}