using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Web.Hoyolab.Bbs.Sign
{
    public static class SignRequestBuilderFactoryExtension
    {
        public static SignRequestBuilderFactory SetGroup(this SignRequestBuilderFactory builder, HoyolabGroup value)
        {
            return builder.Configure(builder.Group = value);
        }
    }
}