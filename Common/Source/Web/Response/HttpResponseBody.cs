using Common.Source.Extension;
using Common.Source.Web.Response.Interface;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Common.Source.Web.Response
{
    public class HttpResponseBody : IResponseCode, IResponseMessage, IResponseSuccess
    {
        [JsonPropertyName("retcode")]
        public int Code { get; }

        [JsonPropertyName("message")]
        public string Message { get; } = string.Empty;

        public virtual bool Success => Code == 0;

        public override string ToString()
        {
            return $"({Code})\x20{Message}";
        }
    }

    public class ResponseWrapper<TContent> : HttpResponseBody, IResponseContent<TContent>
    {
        [JsonPropertyName("data")]
        public TContent? Content { get; set; }

        public bool TryGetContent([NotNullWhen(true)] out TContent? content)
        {
            return Content.IsNotNull(out content) && Success;
        }
    }
}