using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Mission;
using StarRailDamage.Source.Web.Hoyolab.Builder;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMissionInfo : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "mission";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionInfoFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionInfoHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [AID];

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<MissionAnalyzedBody>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            MissionRequestBuilderFactory Factory = new MissionRequestBuilderFactory().SetHoyolabToken(Token);
            FinalizedResponse<MissionResponse> Response = await Factory.Create().SendAsync<MissionResponse>(Program.HttpClient, cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out MissionAnalyzedBody? AnalyzedBody))
            {
                object[] FormatInfo = [AnalyzedBody.TotalPoint, AnalyzedBody.TodayPoint, AnalyzedBody.Mission.GetValueOrDefault(MissionType.Sign), AnalyzedBody.Mission.GetValueOrDefault(MissionType.View), AnalyzedBody.Mission.GetValueOrDefault(MissionType.Upvote), AnalyzedBody.Mission.GetValueOrDefault(MissionType.Share)];
                return TerminalResponse.Create(true, LocalString.WebHoyolabUserMissionInfoContent.Format(FormatInfo), AnalyzedBody);
            }
            return new TerminalResponse<MissionAnalyzedBody>(false, Response.ToString());
        }
    }
}