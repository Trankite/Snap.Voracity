using Common.Source.Extension;
using Common.Source.Web.Request.Builder.Abstraction;

namespace Common.Source.Web.Request.Builder
{
    public static class HttpRequestOptionsBuilderExtension
    {
        public static TBuilder SetOptions<TBuilder, TValue>(this TBuilder builder, HttpRequestOptionsKey<TValue> key, TValue value) where TBuilder : IHttpRequestOptionsBuilder
        {
            return builder.Configure(builder => builder.Options.Set(key, value));
        }
    }
}