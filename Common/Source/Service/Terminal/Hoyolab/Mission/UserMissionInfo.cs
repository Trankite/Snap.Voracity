using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Mission;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMissionInfo : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "mission";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionInfoFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionInfoHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<MissionAnalyzedBody>(SupportTerminalResponse.NotFindToken(aid));
            }
            MissionRequestBuilderFactory Factory = new MissionRequestBuilderFactory().SetHoyolabToken(Token);
            FinalizedResponse<MissionResponse> Response = await Factory.Create().SendAsync<MissionResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out MissionAnalyzedBody? AnalyzedBody))
            {
                object[] FormatInfo = [AnalyzedBody.TotalPoint, AnalyzedBody.TodayPoint, AnalyzedBody.Mission.GetValueOrDefault(MissionType.Sign), AnalyzedBody.Mission.GetValueOrDefault(MissionType.View), AnalyzedBody.Mission.GetValueOrDefault(MissionType.Upvote), AnalyzedBody.Mission.GetValueOrDefault(MissionType.Share)];
                return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabUserMissionInfoContent.SafeFormat(FormatInfo), AnalyzedBody);
            }
            return new TerminalResponse<MissionAnalyzedBody>(false, Response.ToString());
        }
    }
}