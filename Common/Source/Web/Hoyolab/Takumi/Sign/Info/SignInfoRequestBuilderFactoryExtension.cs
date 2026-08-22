using Common.Source.Extension;

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

        public static SignInfoRequestBuilderFactory SetServer(this SignInfoRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Server = value);
        }

        public static SignInfoRequestBuilderFactory SetUid(this SignInfoRequestBuilderFactory builder, string value)
        {
            return builder.Configure(builder.Uid = value);
        }
    }
}