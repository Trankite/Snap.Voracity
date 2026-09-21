using Common.Source.Extension;
using Common.Source.Web.EHentai.Metadata;

namespace Common.Source.Web.EHentai
{
    public static class EHentaiUriHelper
    {
        private const string Host_EHentai = "e-hentai.org";

        private const string Host_ExHentai = "exhentai.org";

        public static EHentaiHost GetHost(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                if (uri.Host.EqualsIgnoreCase(Host_EHentai))
                {
                    return EHentaiHost.EHentai;
                }
                else if (uri.Host.EqualsIgnoreCase(Host_ExHentai))
                {
                    return EHentaiHost.ExHentai;
                }
            }
            return EHentaiHost.None;
        }
    }
}