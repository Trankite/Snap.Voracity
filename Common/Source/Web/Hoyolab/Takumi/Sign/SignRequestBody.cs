using Common.Source.Extension;
using Common.Source.Web.Hoyolab.Metadata;
using System.Text.Json.Serialization;

namespace Common.Source.Web.Hoyolab.Takumi.Sign
{
    public class SignRequestBody
    {
        [JsonPropertyName("act_id")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;

        [JsonPropertyName("lang")]
        public string Language { get; set; } = string.Empty;

        public SignRequestBody() { }

        public SignRequestBody(string action, string region, string uid, string language)
        {
            Action = action;
            Region = region;
            Uid = uid;
            Language = language;
        }

        public static SignRequestBody Create(HoyolabUserRole userRole, HoyolabAction action, HoyolabLanguage language)
        {
            return new SignRequestBody(action.GetDescription(), userRole.Server, userRole.Uid, language.GetDescription());
        }
    }
}