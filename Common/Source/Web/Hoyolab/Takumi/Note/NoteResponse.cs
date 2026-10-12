using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;

namespace Common.Source.Web.Hoyolab.Takumi.Note
{
    public class NoteResponse : ResponseWrapper<NoteResponseWrapper>, IContentSerializer<NoteAnalyzedBody?>
    {
        public NoteAnalyzedBody? Serialize()
        {
            return TryGetContent(out NoteResponseWrapper? Content) ? new NoteAnalyzedBody(Content.CurrentStamina, Content.MaxStamina, DateTimeOffset.FromUnixTimeSeconds(Content.StaminaFullTs).ToLocalTime()) : default;
        }
    }
}