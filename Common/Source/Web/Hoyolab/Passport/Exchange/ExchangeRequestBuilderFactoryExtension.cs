using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public static class ExchangeRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static ExchangeRequestBuilderFactory SetOrigin(this ExchangeRequestBuilderFactory builder, HoyolabTokenType value)
        {
            return builder.Configure(builder.Origin = value);
        }

        [DebuggerStepThrough]
        public static ExchangeRequestBuilderFactory SetDestin(this ExchangeRequestBuilderFactory builder, HoyolabTokenType value)
        {
            return builder.Configure(builder.Destin = value);
        }
    }
}