using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Hoyolab.Takumi.Note;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Game
{
    public class GameStamina : AsyncTerminalCommand<NoteStamina>
    {
        public override string Name => "note";

        public override string FullName => LocalString.ServiceTerminalHoyolabGameStaminaFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabGameStaminaHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<NoteStamina>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<NoteStamina>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? hoyolabToken))
            {
                return new TerminalResponse<NoteStamina>(SupportTerminalResponse.NotFindToken(aid));
            }
            if (!hoyolabToken.TryGetUserRole(HoyolabApp.StarRailChina.OutSender(out HoyolabApp hoyolabApp), out HoyolabUserRole? userRole))
            {
                return new TerminalResponse<NoteStamina>(HoyolabTerminalResponse.NotFindUserRole(hoyolabApp));
            }
            NoteRequestBuilderFactory factory = new NoteRequestBuilderFactory(hoyolabToken).SetUserRole(userRole);
            HttpResponse<NoteResponse> response = await factory.Create().SendAsync<NoteResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TrySerialize(out NoteStamina? noteStamina))
            {
                TimeSpan timeSpan = noteStamina.FullTime.Subtract(DateTimeOffset.Now);
                object?[] formatInfo = [noteStamina.Current, noteStamina.Maximum, (int)timeSpan.TotalHours, timeSpan.Minutes];
                return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabGameStaminaContent.SafeFormat(formatInfo), noteStamina);
            }
            return new TerminalResponse<NoteStamina>(false, response.ToString());
        }
    }
}