using System.Text;

namespace Common.Source.Web.Request.Interface
{
    public interface IHttpContentSerializer
    {
        HttpContent? Serialize<TSender>(TSender sender, Encoding? encoding = default);

        ValueTask<TResult?> DeserializeAsync<TResult>(HttpContent httpContent, CancellationToken cancellationToken = default);
    }

    public interface IHttpContentSerializer<T> : IHttpContentSerializer
    {
        HttpContent? Serialize(T sender, Encoding? encoding = default);

        ValueTask<T?> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default);
    }
}