using Common.Source.Extension;

namespace Common.Source.Web.Hoyolab.Builder
{
    public static class HoyolabHttpRequestMessageBuilderFactoryExtension
    {
        public static T SetHoyolabToken<T>(this T builder, HoyolabToken hoyolabToken) where T : HoyolabHttpRequestMessageBuilderFactory
        {
            return builder.Configure(builder.HoyolabToken = hoyolabToken);
        }
    }
}