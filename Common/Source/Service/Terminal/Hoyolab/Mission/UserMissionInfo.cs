using Common.Source.Extension;
using Common.Source.Factory.Serialize.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Mission;
using Common.Source.Web.Hoyolab.Builder;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMissionInfo : AsyncTerminalCommand<MissionInfo>
    {
        public override string Name => "mission";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionInfoFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionInfoHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? hoyolabToken))
            {
                return new TerminalResponse<MissionInfo>(SupportTerminalResponse.NotFindToken(aid));
            }
            MissionRequestBuilderFactory factory = new MissionRequestBuilderFactory().SetHoyolabToken(hoyolabToken);
            HttpResponse<MissionResponse> response = await factory.Create().SendAsync<MissionResponse>(cancellationToken).ConfigureAwait(false);
            if (response.Body.IsNotNull() && response.Body.TrySerialize(out MissionInfo? missionInfo))
            {
                object[] formatInfo = [missionInfo.TotalPoint, missionInfo.TodayPoint, missionInfo.Mission.GetValueOrDefault(MissionType.Sign), missionInfo.Mission.GetValueOrDefault(MissionType.View), missionInfo.Mission.GetValueOrDefault(MissionType.Upvote), missionInfo.Mission.GetValueOrDefault(MissionType.Share)];
                return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabUserMissionInfoContent.SafeFormat(formatInfo), missionInfo);
            }
            return new TerminalResponse<MissionInfo>(false, response.ToString());
        }
    }
}