using Common.Source.Extension;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public static class ExchangeRequestBuilderFactoryExtension
    {
        public static ExchangeRequestBuilderFactory SetOrigin(this ExchangeRequestBuilderFactory builder, HoyolabTokenType value)
        {
            return builder.Configure(builder.Origin = value);
        }

        public static ExchangeRequestBuilderFactory SetDestin(this ExchangeRequestBuilderFactory builder, HoyolabTokenType value)
        {
            return builder.Configure(builder.Destin = value);
        }
    }
}