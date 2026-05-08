namespace StarRailDamage.Source.Web.Hoyolab.Passport.Exchange
{
    public static class ExchangeTypeExtension
    {
        public static string GetToken(this ExchangeType exchangeType, HoyolabToken hoyolabToken)
        {
            return hoyolabToken.GetToken(exchangeType.GetTokenType());
        }

        public static HoyolabTokenType GetTokenType(this ExchangeType exchangeType)
        {
            return exchangeType switch { ExchangeType.SToken => HoyolabTokenType.SToken, ExchangeType.LToken => HoyolabTokenType.LToken, ExchangeType.Cookie => HoyolabTokenType.Cookie, _ => default };
        }
    }
}