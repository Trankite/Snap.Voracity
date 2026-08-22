namespace Common.Source.Web.Request.Builder.Abstraction
{
    public interface IHttpContentBuilder
    {
        HttpContent? Content { get; set; }

        IHttpContentSerializer HttpContentSerializer { get; }
    }
}