using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Web.Hoyolab.Takumi.Sign.Home
{
    public static class SignHomeRequestBuilderFactoryExtension
    {
        public static SignHomeRequestBuilderFactory SetAction(this SignHomeRequestBuilderFactory builder, HoyolabAction value)
        {
            return builder.Configure(builder.Action = value);
        }

        public static SignHomeRequestBuilderFactory SetLanguage(this SignHomeRequestBuilderFactory builder, HoyolabLanguage value)
        {
            return builder.Configure(builder.Language = value);
        }
    }
}