using Common.Source.Extension;
using Common.Source.Factory.Streams.Html;
using Common.Source.Web.Request.Builder.Abstraction;
using System.Text;
using System.Text.Json;

namespace Common.Source.Web.EHentai.Slide
{
    public class SlideHttpContentSerializer : IHttpContentSerializer<SlideAnalyzedBody>
    {
        public HttpContent Serialize(SlideAnalyzedBody content, Encoding? encoding = null)
        {
            return new StringContent(JsonSerializer.Serialize(content), encoding);
        }

        public async ValueTask<SlideAnalyzedBody> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            SlideAnalyzedBody AnalyzedBody = new();
            using HtmlReader Reader = new(httpContent.ReadAsStream(cancellationToken));
            using IEnumerator<HtmlElement> Enumerator = Reader.GetEnumerator();
            while (Enumerator.TryMoveNext(out HtmlElement? HtmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (HtmlElement.Markup.EqualsIgnoreCase(HtmlMarkup.Title))
                {
                    AnalyzedBody.Title = HtmlElement.Contents.FirstOrDefault().NotNull();
                }
                else if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "img")
                {
                    return AnalyzedBody.Configure(AnalyzedBody.ImageUrl = HtmlElement.GetAttributeOrDefault(HtmlAttribute.Source, string.Empty));
                }
            }
            return AnalyzedBody;
        }
    }
}