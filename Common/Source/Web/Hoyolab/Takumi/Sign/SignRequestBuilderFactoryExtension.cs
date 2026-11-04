using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Sign
{
    public static class SignRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static SignRequestBuilderFactory SetBody(this SignRequestBuilderFactory builder, SignRequestBody body)
        {
            return builder.Configure(builder.Body = body);
        }
    }
}