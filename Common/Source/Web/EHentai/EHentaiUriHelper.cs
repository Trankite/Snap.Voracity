using Common.Source.Extension;
using Common.Source.Web.EHentai.Metadata;

namespace Common.Source.Web.EHentai
{
    public static class EHentaiUriHelper
    {
        private const string EHentaiHost = "e-hentai.org";

        private const string ExHentaiHost = "exhentai.org";

        public static EHentaiHost GetHost(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                if (uri.Host.EqualsIgnoreCase(EHentaiHost))
                {
                    return Metadata.EHentaiHost.EHentai;
                }
                else if (uri.Host.EqualsIgnoreCase(ExHentaiHost))
                {
                    return Metadata.EHentaiHost.ExHentai;
                }
            }
            return default;
        }
    }
}