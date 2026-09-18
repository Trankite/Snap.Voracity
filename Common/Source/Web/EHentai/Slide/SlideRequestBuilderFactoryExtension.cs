using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Slide
{
    public static class SlideRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static SlideRequestBuilderFactory SetUri(this SlideRequestBuilderFactory builder, Uri? uri)
        {
            return builder.Configure(builder.Uri = uri);
        }
    }
}