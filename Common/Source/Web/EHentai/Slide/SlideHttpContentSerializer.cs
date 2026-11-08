using Common.Source.Extension;
using Common.Source.Service.Streams.Html;
using Common.Source.Service.Streams.Html.Metadata;
using Common.Source.Web.Request.Abstract;

namespace Common.Source.Web.EHentai.Slide
{
    public class SlideHttpContentSerializer : HttpContentSerializer<SlideInfo>
    {
        public override async ValueTask<SlideInfo?> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            SlideInfo slideInfo = new();
            using HtmlReader reader = new(httpContent.ReadAsStream(cancellationToken));
            await using IAsyncEnumerator<HtmlElement> asyncEnumerator = reader.GetAsyncEnumerator(cancellationToken);
            while ((await asyncEnumerator.TryMoveNextAsync().ConfigureAwait(false)).TryGetValue(out HtmlElement? htmlElement))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (htmlElement.Name.EqualsIgnoreCase(HtmlMarkup.Title))
                {
                    slideInfo.Title = htmlElement.Contents.FirstOrDefault().NotNull();
                }
                else if (htmlElement.GetAttributeOrDefault(HtmlAttribute.Id) == "img")
                {
                    return slideInfo.Configure(slideInfo.ImageUrl = htmlElement.GetAttributeOrDefault(HtmlAttribute.Source).NotNull());
                }
            }
            return slideInfo;
        }
    }
}