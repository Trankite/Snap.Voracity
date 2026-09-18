using Common.Source.Web.Request.Builder;

namespace Common.Source.Web.EHentai.Builder
{
    public class EHentaiCookieBuilder : HttpCookiesBuilder
    {
        public EHentaiToken EHentaiToken { get; set; }

        public EHentaiCookieBuilder(EHentaiToken? eHentaiToken = default)
        {
            EHentaiToken = eHentaiToken ?? new EHentaiToken();
        }
    }
}