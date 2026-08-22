using Common.Source.Extension;
using Common.Source.Web.Request.Builder.Abstraction;
using System.Diagnostics;

namespace Common.Source.Web.Request.Builder
{
    public static class HttpMethodBuilderExtension
    {
        [DebuggerStepThrough]
        public static T SetMethod<T>(this T builder, HttpMethod method) where T : IHttpMethodBuilder
        {
            return builder.Configure(builder.Method = method);
        }
    }
}