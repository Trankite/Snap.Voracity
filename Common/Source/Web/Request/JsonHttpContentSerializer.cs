using Common.Source.Extension;
using Common.Source.Web.Request.Builder.Abstraction;
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

        public HttpContent? Serialize<TContent>(TContent? content, Encoding? encoding = default)
        {
            return new StringContent(JsonSerializer.Serialize(content, JsonSerializerOptions), encoding, MediaTypeNames.Application.Json);
        }

        public async ValueTask<TResult?> DeserializeAsync<TResult>(HttpContent? httpContent, CancellationToken cancellationToken = default)
        {
            return httpContent.IsNotNull() ? await JsonSerializer.DeserializeAsync<TResult>(httpContent.ReadAsStream(cancellationToken), JsonSerializerOptions, cancellationToken).ConfigureAwait(false) : default;
        }
    }
}