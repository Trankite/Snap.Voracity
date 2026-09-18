using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Takumi.Note;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameStamina : AsyncTerminalCommand<NoteAnalyzedBody>
    {
        public override string Name => "note";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameStaminaFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameStaminaHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<NoteAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<NoteAnalyzedBody>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<NoteAnalyzedBody>(SupportTerminalResponse.NotFindToken(aid));
            }
            if (!Token.TryGetUserRole(HoyolabApp.StarRailChina.OutSelf(out HoyolabApp Game), out HoyolabUserRole? UserRole))
            {
                return new TerminalResponse<NoteAnalyzedBody>(HoyolabTerminalResponse.NotFindUserRole(Game));
            }
            NoteRequestBuilderFactory Factory = new NoteRequestBuilderFactory(Token).SetUserRole(UserRole);
            FinalizedResponse<NoteResponse> Response = await Factory.Create().SendAsync<NoteResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out NoteAnalyzedBody? Body))
            {
                TimeSpan Offset = Body.FullTime.Subtract(DateTimeOffset.Now);
                return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabGameStaminaContent.SafeFormat(Body.Current, Body.Maximum, (int)Offset.TotalHours, Offset.Minutes), Body);
            }
            return new TerminalResponse<NoteAnalyzedBody>(false, Response.ToString());
        }
    }
}