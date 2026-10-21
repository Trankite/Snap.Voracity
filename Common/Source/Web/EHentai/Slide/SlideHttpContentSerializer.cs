using Common.Source.Extension;
using Common.Source.Factory.Streams.Html;
using Common.Source.Factory.Streams.Html.Metadata;
using Common.Source.Web.Request.Abstract;

namespace Common.Source.Web.EHentai.Slide
{
    public class SlideHttpContentSerializer : HttpContentSerializer<SlideAnalyzedBody>
    {
        public override async ValueTask<SlideAnalyzedBody?> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            SlideAnalyzedBody AnalyzedBody = new();
            using HtmlReader Reader = new(httpContent.ReadAsStream(cancellationToken));
            await using IAsyncEnumerator<HtmlElement> AsyncEnumerator = Reader.GetAsyncEnumerator(cancellationToken);
            while ((await AsyncEnumerator.MoveAsync().ConfigureAwait(false)).TryGetValue(out HtmlElement? HtmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (HtmlElement.Name.EqualsIgnoreCase(HtmlMarkup.Title))
                {
                    AnalyzedBody.Title = HtmlElement.Contents.FirstOrDefault().NotNull();
                }
                else if (HtmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "img")
                {
                    return AnalyzedBody.Configure(AnalyzedBody.ImageUrl = HtmlElement.GetAttributeOrDefault(HtmlAttribute.Source).NotNull());
                }
            }
            return AnalyzedBody;
        }
    }
}