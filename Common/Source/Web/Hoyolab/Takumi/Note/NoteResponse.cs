using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Note
{
    public class NoteResponse : ResponseWrapper<NoteResponseWrapper>, IContentSerializer<NoteStamina?>
    {
        public NoteStamina? Serialize()
        {
            return TryGetContent(out NoteResponseWrapper? content) ? new NoteStamina(content.CurrentStamina, content.MaxStamina, DateTimeOffset.FromUnixTimeSeconds(content.StaminaFullTs).ToLocalTime()) : default;
        }
    }
}