using StarRailDamage.Source.Extension;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace StarRailDamage.Source.Web.Response
{
    public class ResponseWrapper : IResponseMessage, IResponseValidator
    {
        [JsonPropertyName("retcode")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        public virtual bool IsSuccess() => Code == 0;

        public override string ToString()
        {
            return $"{Message} ({Code})";
        }
    }

    public class ResponseWrapper<TContent> : ResponseWrapper, IResponseAnalyzedBody<TContent>
    {
        [JsonPropertyName("data")]
        public TContent? Content { get; set; }

        public virtual bool TryGetAnalyzedBody([NotNullWhen(true)] out TContent? analyedBody)
        {
            return IsSuccess() && Content.IsNotNull() ? true.Configure(analyedBody = Content) : false.Configure(analyedBody = default);
        }
    }

    public abstract class ResponseWrapper<TContent, TAnalyzedBody> : ResponseWrapper<TContent>, IResponseAnalyzedBody<TAnalyzedBody>
    {
        public abstract bool TryGetAnalyzedBody([NotNullWhen(true)] out TAnalyzedBody? analyedBody);
    }
}