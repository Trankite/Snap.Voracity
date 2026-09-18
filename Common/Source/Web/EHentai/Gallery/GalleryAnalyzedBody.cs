namespace Common.Source.Web.EHentai.Gallery
{
    public class GalleryAnalyzedBody
    {
        public bool IsEndOfPage { get; set; }

        public IList<string> Images { get; set; } = [];
    }
}