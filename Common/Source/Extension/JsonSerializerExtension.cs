using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Common.Source.Extension
{
    public static class JsonSerializerExtension
    {
        public static readonly JsonSerializerOptions JsonOptions;

        [DebuggerStepThrough]
        public static JsonSerializerOptions Copy(this JsonSerializerOptions options)
        {
            return new JsonSerializerOptions(options);
        }

        [DebuggerStepThrough]
        public static string Serialize<T>(this T content, JsonSerializerOptions? options = default)
        {
            return JsonSerializer.Serialize(content, options ?? JsonOptions);
        }

        [DebuggerStepThrough]
        public static void Serialize<T>(Stream utf8Json, T value, JsonSerializerOptions? options = default)
        {
            JsonSerializer.Serialize(utf8Json, value, options ?? JsonOptions);
        }

        [DebuggerStepThrough]
        public static Task SerializeAsync<T>(Stream utf8Json, T value, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
        {
            return JsonSerializer.SerializeAsync(utf8Json, value, options ?? JsonOptions, cancellationToken);
        }

        [DebuggerStepThrough]
        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = default)
        {
            return JsonSerializer.Deserialize<T>(json, options ?? JsonOptions);
        }

        [DebuggerStepThrough]
        public static T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? options = default)
        {
            return JsonSerializer.Deserialize<T>(utf8Json, options ?? JsonOptions);
        }

        [DebuggerStepThrough]
        public static async ValueTask<T?> DeserializeAsync<T>(Stream utf8Json, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(utf8Json, options ?? JsonOptions, cancellationToken).ConfigureAwait(false);
        }

        static JsonSerializerExtension()
        {
            JsonOptions = new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        }
    }
}