using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeResponse : ResponseWrapper<ExchangeResponseWrapper, ExchangeResponseToken>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out ExchangeResponseToken? analyedBody)
        {
            return TryGetAnalyzedBody(out ExchangeResponseWrapper? Content) ? true.Configure(analyedBody = Content.Token) : false.Configure(analyedBody = default);
        }
    }
}