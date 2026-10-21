using Common.Source.Web.Request.Interface;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Common.Source.Web.Request.Abstract
{
    public abstract class HttpContentSerializer<T> : IHttpContentSerializer<T>
    {
        public virtual HttpContent? Serialize<TSender>(TSender sender, Encoding? encoding = default)
        {
            return new StringContent(JsonSerializer.Serialize(sender), encoding, MediaTypeNames.Application.Json);
        }

        public virtual HttpContent? Serialize(T sender, Encoding? encoding = default)
        {
            return Serialize<T>(sender, encoding);
        }

        public virtual async ValueTask<TResult?> DeserializeAsync<TResult>(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            return await DeserializeAsync(httpContent, cancellationToken).ConfigureAwait(false) is TResult result ? result : default;
        }

        public abstract ValueTask<T?> DeserializeAsync(HttpContent httpContent, CancellationToken cancellationToken = default);
    }
}