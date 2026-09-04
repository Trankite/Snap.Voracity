using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Web.Hoyolab.Takumi.Sign.Info
{
    public static class SignInfoRequestBuilderFactoryExtension
    {
        public static SignInfoRequestBuilderFactory SetAction(this SignInfoRequestBuilderFactory builder, HoyolabAction value)
        {
            return builder.Configure(builder.Action = value);
        }

        public static SignInfoRequestBuilderFactory SetLanguage(this SignInfoRequestBuilderFactory builder, HoyolabLanguage value)
        {
            return builder.Configure(builder.Language = value);
        }

        public static SignInfoRequestBuilderFactory SetUserRole(this SignInfoRequestBuilderFactory builder, HoyolabUserRole value)
        {
            return builder.Configure(builder.UserRole = value);
        }
    }
}