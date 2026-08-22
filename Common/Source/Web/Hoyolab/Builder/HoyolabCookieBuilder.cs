using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.Hoyolab.Builder
{
    public class HoyolabCookieBuilder : HttpCookiesBuilder
    {
        public HoyolabToken HoyolabToken { get; set; }

        public HoyolabCookieBuilder()
        {
            HoyolabToken = new HoyolabToken();
        }

        public HoyolabCookieBuilder(HoyolabToken hoyolabToken)
        {
            HoyolabToken = hoyolabToken;
        }
    }
}