using Common.Source.Extension;
using Common.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab.Takumi.Note
{
    public class NoteResponse : ResponseWrapper<NoteResponseWrapper, NoteAnalyzedBody>
    {
        public override bool TryGetAnalyzedBody([NotNullWhen(true)] out NoteAnalyzedBody? analyedBody)
        {
            return TryGetAnalyzedBody(out NoteResponseWrapper? Content) ? true.Configure(analyedBody = new(Content.CurrentStamina, Content.MaxStamina, DateTimeOffset.FromUnixTimeSeconds(Content.StaminaFullTs).ToLocalTime())) : false.Configure(analyedBody = default);
        }
    }
}