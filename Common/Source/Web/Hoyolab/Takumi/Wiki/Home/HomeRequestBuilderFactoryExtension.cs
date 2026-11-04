using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Takumi.Wiki.Home
{
    public static class HomeRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static HomeRequestBuilderFactory SetChannelType(this HomeRequestBuilderFactory builder, ChannelType value)
        {
            return builder.Configure(builder.ChannelType = value);
        }
    }
}