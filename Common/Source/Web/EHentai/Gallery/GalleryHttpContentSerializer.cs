using Common.Source.Extension;
using Common.Source.Factory.Streams.Html;
using Common.Source.Factory.Streams.Html.Metadata;
using Common.Source.Web.Request.Abstract;

namespace Common.Source.Web.EHentai.Gallery
{
    public class GalleryHttpContentSerializer : HttpContentSerializer<GalleryFragment>
    {
        public override async ValueTask<GalleryFragment?> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            GalleryFragment galleryFragment = new();
            using HtmlReader reader = new(httpContent.ReadAsStream(cancellationToken));
            await using IAsyncEnumerator<HtmlElement> asyncEnumerator = reader.GetAsyncEnumerator(cancellationToken);
            while ((await asyncEnumerator.TryMoveNextAsync().ConfigureAwait(false)).TryGetValue(out HtmlElement? htmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (htmlElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptt")
                {
                    if (htmlElement.Elements.TryGetFirst(out HtmlElement? tableElement))
                    {
                        if (tableElement.Elements.TryGetLast(out HtmlElement? endedElement))
                        {
                            galleryFragment.IsEndOfPage = endedElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptdd";
                        }
                    }
                }
                else if (htmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "gdt")
                {
                    foreach (HtmlElement currentElement in htmlElement.Elements)
                    {
                        galleryFragment.Images.Add(currentElement.GetAttributeOrDefault(HtmlAttribute.Href).NotNull());
                    }
                    return galleryFragment;
                }
            }
            return galleryFragment;
        }
    }
}