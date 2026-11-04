using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public static class SignHomeRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static SignHomeRequestBuilderFactory SetAction(this SignHomeRequestBuilderFactory builder, HoyolabAction value)
        {
            return builder.Configure(builder.Action = value);
        }

        [DebuggerStepThrough]
        public static SignHomeRequestBuilderFactory SetLanguage(this SignHomeRequestBuilderFactory builder, HoyolabLanguage value)
        {
            return builder.Configure(builder.Language = value);
        }
    }
}