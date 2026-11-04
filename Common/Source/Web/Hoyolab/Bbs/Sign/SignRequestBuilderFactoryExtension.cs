using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Bbs.Sign
{
    public static class SignRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static SignRequestBuilderFactory SetGroup(this SignRequestBuilderFactory builder, HoyolabGroup value)
        {
            return builder.Configure(builder.Group = value);
        }
    }
}