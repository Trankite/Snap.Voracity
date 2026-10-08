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
            using IEnumerator<HtmlElement> Enumerator = Reader.GetEnumerator();
            while (Enumerator.TryMoveNext(out HtmlElement? HtmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptt")
                {
                    if (HtmlElement.Elements.TryGetFirst(out HtmlElement? TableElement))
                    {
                        if (TableElement.Elements.TryGetLast(out HtmlElement? EndedElement))
                        {
                            AnalyzedBody.IsEndOfPage = EndedElement.GetAttributeOrDefault(HtmlAttribute.Class) == "ptdd";
                        }
                    }
                }
                else if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "gdt")
                {
                    foreach (HtmlElement CurrentElement in HtmlElement.Elements)
                    {
                        AnalyzedBody.Images.Add(CurrentElement.GetAttributeOrDefault(HtmlAttribute.Href, string.Empty));
                    }
                    return AnalyzedBody;
                }
            }
            return AnalyzedBody;
        }
    }
}