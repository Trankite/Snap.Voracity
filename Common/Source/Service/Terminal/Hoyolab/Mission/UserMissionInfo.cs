using Common.Source.Extension;
using Common.Source.Factory.Serialize;
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

namespace Common.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMissionInfo : AsyncTerminalCommand<MissionInfo>
    {
        public override string Name => "mission";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionInfoFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionInfoHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<MissionInfo>(SupportTerminalResponse.NotFindToken(aid));
            }
            MissionRequestBuilderFactory Factory = new MissionRequestBuilderFactory().SetHoyolabToken(Token);
            HttpResponse<MissionResponse> Response = await Factory.Create().SendAsync<MissionResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TrySerialize(out MissionInfo? MissionInfo))
            {
                object[] FormatInfo = [MissionInfo.TotalPoint, MissionInfo.TodayPoint, MissionInfo.Mission.GetValueOrDefault(MissionType.Sign), MissionInfo.Mission.GetValueOrDefault(MissionType.View), MissionInfo.Mission.GetValueOrDefault(MissionType.Upvote), MissionInfo.Mission.GetValueOrDefault(MissionType.Share)];
                return TerminalResponse.Create(true, LocalString.ServiceTerminalHoyolabUserMissionInfoContent.SafeFormat(FormatInfo), MissionInfo);
            }
            return new TerminalResponse<MissionInfo>(false, Response.ToString());
        }
    }
}