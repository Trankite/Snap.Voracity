using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace StarRailDamage.Source.Extension
{
    public static class JsonSerializerExtension
    {
        public static readonly JsonSerializerOptions JsonOptions;

        public static JsonSerializerOptions Copy(this JsonSerializerOptions options)
        {
            return new JsonSerializerOptions(options);
        }

        public static string Serialize<T>(this T content, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(content, options ?? JsonOptions);
        }

        public static Task SerializeAsync<T>(Stream utf8Json, T value, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.SerializeAsync(utf8Json, value, options ?? JsonOptions);
        }

        public static T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<T>(json, options ?? JsonOptions);
        }

        public static T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<T>(utf8Json, options ?? JsonOptions);
        }

        static JsonSerializerExtension()
        {
            JsonOptions = new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        }
    }
}