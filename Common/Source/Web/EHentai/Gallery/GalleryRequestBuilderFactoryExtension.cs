using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.EHentai.Gallery
{
    public static class GalleryRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static GalleryRequestBuilderFactory SetUrl(this GalleryRequestBuilderFactory builder, Uri uri)
        {
            return builder.Configure(builder.Uri = uri);
        }

        [DebuggerStepThrough]
        public static GalleryRequestBuilderFactory SetPage(this GalleryRequestBuilderFactory builder, int page)
        {
            return builder.Configure(builder.Page = page);
        }
    }
}