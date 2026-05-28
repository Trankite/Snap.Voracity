using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Takumi.Note;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game
{
    public class GameStamina : AsyncTerminalCommand<NoteAnalyzedBody>
    {
        public override string Name => "note";

        public override string Help => LocalString.ServiceTerminalHoyolabGameStaminaHelp;

        public override string[] Parameters => [AID];

        private const string AID = "aid";

        protected override async ValueTask<ITerminalResponse<NoteAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse<NoteAnalyzedBody>> AsyncInvoke(string? aid = null)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<NoteAnalyzedBody>(HoyolabTerminalResponse.NotFindToken(aid));
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
                return TerminalResponse.Create(true, LocalString.WebHoyolabGameStaminaContent.Format(Body.Current, Body.Maximum, (int)Offset.TotalHours, Offset.Minutes), Body);
            }
            return new TerminalResponse<NoteAnalyzedBody>(false, Response.ToString());
        }
    }
}