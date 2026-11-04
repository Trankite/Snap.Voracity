using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Info
{
    public static class SignInfoRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static SignInfoRequestBuilderFactory SetAction(this SignInfoRequestBuilderFactory builder, HoyolabAction value)
        {
            return builder.Configure(builder.Action = value);
        }

        [DebuggerStepThrough]
        public static SignInfoRequestBuilderFactory SetLanguage(this SignInfoRequestBuilderFactory builder, HoyolabLanguage value)
        {
            return builder.Configure(builder.Language = value);
        }

        [DebuggerStepThrough]
        public static SignInfoRequestBuilderFactory SetUserRole(this SignInfoRequestBuilderFactory builder, HoyolabUserRole value)
        {
            return builder.Configure(builder.UserRole = value);
        }
    }
}