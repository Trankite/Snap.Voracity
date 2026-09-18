using Common.Source.Extension;
using Common.Source.Factory.Streams.Html;
using Common.Source.Web.Request.Builder.Abstraction;
using System.Text;
using System.Text.Json;

namespace Common.Source.Web.EHentai.Gallery
{
    public class GalleryHttpContentSerializer : IHttpContentSerializer<GalleryAnalyzedBody>
    {
        public HttpContent Serialize(GalleryAnalyzedBody content, Encoding? encoding = null)
        {
            return new StringContent(JsonSerializer.Serialize(content), encoding);
        }

        public async ValueTask<GalleryAnalyzedBody> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            GalleryAnalyzedBody AnalyzedBody = new();
            using HtmlReader Reader = new(httpContent.ReadAsStream(cancellationToken));
            IEnumerator<HtmlElement> Enumerator = Reader.GetEnumerator();
            while (Enumerator.TryMoveNext(out HtmlElement? HtmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptt")
                {
                    SetPageInfoFromHtmlElement(HtmlElement, AnalyzedBody);
                }
                else if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "gdt")
                {
                    SetImagesFromHtmlElement(HtmlElement, AnalyzedBody);
                }
            }
            return AnalyzedBody;
        }

        private static void SetPageInfoFromHtmlElement(HtmlElement htmlElement, GalleryAnalyzedBody analyzedBody)
        {
            if (!htmlElement.Elements.TryGetFirst(out HtmlElement? TableElement))
            {
                return;
            }
            if (TableElement.Elements.TryGetLast(out HtmlElement? EndedElement))
            {
                analyzedBody.IsEndOfPage = EndedElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptdd";
            }
        }

        private static void SetImagesFromHtmlElement(HtmlElement htmlElement, GalleryAnalyzedBody analyzedBody)
        {
            foreach (HtmlElement CurrentElement in htmlElement.Elements)
            {
                analyzedBody.Images.Add(CurrentElement.GetAttributeOrDefault(HtmlAttribute.Href).NotNull());
            }
        }
    }
}