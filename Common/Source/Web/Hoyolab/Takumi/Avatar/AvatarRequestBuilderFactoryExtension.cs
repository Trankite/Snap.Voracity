using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Avatar
{
    public static class AvatarRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static AvatarRequestBuilderFactory SetUid(this AvatarRequestBuilderFactory builder, string uid)
        {
            return builder.Configure(builder.Uid = uid);
        }

        [DebuggerStepThrough]
        public static AvatarRequestBuilderFactory SetServer(this AvatarRequestBuilderFactory builder, string server)
        {
            return builder.Configure(builder.Server = server);
        }
    }
}