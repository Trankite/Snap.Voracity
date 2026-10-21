using Common.Source.Web.Request.Abstract;

namespace Common.Source.Web.Request.Interface
{
    public interface IHttpRequestMessageBuilderFactory
    {
        HttpRequestMessageBuilder Create();
    }
}