using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Passport.Exchange
{
    public class ExchangeResponse : ResponseWrapper<ExchangeResponseWrapper>, IContentSerializer<ExchangeResponseToken?>
    {
        public ExchangeResponseToken? Serialize()
        {
            return TryGetContent(out ExchangeResponseWrapper? Content) ? Content.Token : default;
        }
    }
}