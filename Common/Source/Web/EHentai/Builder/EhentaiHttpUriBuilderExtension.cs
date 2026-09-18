using Common.Source.Web.Request.Builder.Abstraction;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Builder
{
    public static class EhentaiHttpUriBuilderExtension
    {
        [DebuggerStepThrough]
        public static EhentaiHttpUriBuilder SetPage(this EhentaiHttpUriBuilder builder, int page)
        {
            return builder.SetQuery("p", page.ToString());
        }
    }
}