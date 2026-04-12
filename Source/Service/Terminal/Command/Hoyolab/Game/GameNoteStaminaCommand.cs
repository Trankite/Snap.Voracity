using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Takumi.Note;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game
{
    public class GameNoteStaminaCommand : AsyncTerminalCommand<NoteAnalyzedBody>
    {
        public override string Name => "note";

        public override string Help => MarkedText.HoyolabGameNoteStaminaCommandHelp;

        protected override async ValueTask<ITerminalResponse<NoteAnalyzedBody>> AsyncInvokeOverride(params IList<string> parameter)
        {
            string? AidText = parameter.FirstOrDefault();
            if (!HoyolabTokenManage.TryGetTokenOrFirst(AidText, out HoyolabToken? Token))
            {
                return new TerminalResponse<NoteAnalyzedBody>(HoyolabTerminalResponse.NotFindToken(AidText));
            }
            if (!Token.TryGetUserRole(GameType.StarRailChina.OutSelf(out GameType Game), out HoyolabUserRole? UserRole))
            {
                return new TerminalResponse<NoteAnalyzedBody>(HoyolabTerminalResponse.NotFindUserRole(Game));
            }
            NoteRequestBuilderFactory Factory = new NoteRequestBuilderFactory(Token).SetUserRole(UserRole);
            FinalizedResponse<NoteResponse> Response = await Factory.Create().SendAsync<NoteResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out NoteAnalyzedBody? Body))
            {
                TimeSpan Offset = Body.FullTime.Subtract(DateTimeOffset.Now);
                return TerminalResponse.Create(true, StringExtension.Format(MarkedText.HoyolabGameNoteStamina, Body.Current, Body.Maximum, (int)Offset.TotalHours, Offset.Minutes), Body);
            }
            return new TerminalResponse<NoteAnalyzedBody>(false, Response.ToString());
        }
    }
}