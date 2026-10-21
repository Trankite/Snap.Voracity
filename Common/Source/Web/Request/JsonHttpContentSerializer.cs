using Common.Source.Web.Request.Interface;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Common.Source.Web.Request
{
    public class JsonHttpContentSerializer : IHttpContentSerializer
    {
        public JsonSerializerOptions? JsonSerializerOptions { get; }

        public JsonHttpContentSerializer() { }

        public JsonHttpContentSerializer(JsonSerializerOptions? jsonSerializerOptions)
        {
            JsonSerializerOptions = jsonSerializerOptions;
        }

        public HttpContent? Serialize<TSender>(TSender sender, Encoding? encoding = default)
        {
            return new StringContent(JsonSerializer.Serialize(sender, JsonSerializerOptions), encoding, MediaTypeNames.Application.Json);
        }

        public async ValueTask<TResult?> DeserializeAsync<TResult>(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<TResult>(httpContent.ReadAsStream(cancellationToken), JsonSerializerOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}