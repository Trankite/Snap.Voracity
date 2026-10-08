using Common.Source.Web.Request.Builder.Abstraction;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Builder
{
    public static class EHentaiHttpUriBuilderExtension
    {
        [DebuggerStepThrough]
        public static EHentaiHttpUriBuilder SetPage(this EHentaiHttpUriBuilder builder, int page)
        {
            return builder.SetQuery("p", page.ToString());
        }
    }
}