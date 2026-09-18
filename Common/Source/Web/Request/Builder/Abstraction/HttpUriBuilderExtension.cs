using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Request.Builder.Abstraction
{
    public static class HttpUriBuilderExtension
    {
        [DebuggerStepThrough]
        public static T SetQuery<T>(this T builder, string name, string value) where T : HttpUriBuilder
        {
            return builder.Configure(builder.Query[name] = value);
        }
    }
}