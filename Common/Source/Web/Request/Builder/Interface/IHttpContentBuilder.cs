using Common.Source.Web.Request.Interface;

namespace Common.Source.Web.Request.Builder.Interface
{
    public interface IHttpContentBuilder
    {
        HttpContent? Content { get; set; }

        IHttpContentSerializer HttpContentSerializer { get; }
    }
}