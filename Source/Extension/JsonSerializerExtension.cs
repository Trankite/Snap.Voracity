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

        static JsonSerializerExtension()
        {
            JsonOptions = new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        }
    }
}