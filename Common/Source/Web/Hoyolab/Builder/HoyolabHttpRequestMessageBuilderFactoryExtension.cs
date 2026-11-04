using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Builder
{
    public static class HoyolabHttpRequestMessageBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static T SetHoyolabToken<T>(this T builder, HoyolabToken hoyolabToken) where T : HoyolabHttpRequestMessageBuilderFactory
        {
            return builder.Configure(builder.HoyolabToken = hoyolabToken);
        }
    }
}