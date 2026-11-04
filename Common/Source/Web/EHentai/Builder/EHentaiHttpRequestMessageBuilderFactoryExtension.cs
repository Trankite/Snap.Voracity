using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Builder
{
    public static class EHentaiHttpRequestMessageBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static T SetEHentaiToken<T>(this T builder, EHentaiToken? ehentaiToken) where T : EHentaiHttpRequestMessageBuilderFactory
        {
            return builder.Configure(builder.EHentaiToken = ehentaiToken);
        }
    }
}