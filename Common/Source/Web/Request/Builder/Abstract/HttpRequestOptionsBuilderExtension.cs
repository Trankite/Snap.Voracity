using Common.Source.Extension;
using Common.Source.Web.Request.Builder.Interface;
using System.Diagnostics;

namespace Common.Source.Web.Request.Builder.Abstract
{
    public static class HttpRequestOptionsBuilderExtension
    {
        [DebuggerStepThrough]
        public static TBuilder SetOptions<TBuilder, TValue>(this TBuilder builder, HttpRequestOptionsKey<TValue> key, TValue value) where TBuilder : IHttpRequestOptionsBuilder
        {
            return builder.Configure(builder => builder.Options.Set(key, value));
        }
    }
}