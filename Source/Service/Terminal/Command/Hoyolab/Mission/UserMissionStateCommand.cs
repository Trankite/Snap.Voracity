using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Mission;
using StarRailDamage.Source.Web.Hoyolab.Builder;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission
{
    public class UserMissionStateCommand : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "mission";

        public override string Help => MarkedText.HoyolabUserMissionStateCommandHelp;

        protected override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(params IList<string> parameter)
        {
            string? AidText = parameter.FirstOrDefault();
            if (!HoyolabTokenManage.TryGetTokenOrFirst(AidText, out HoyolabToken? Token))
            {
                return new TerminalResponse<MissionAnalyzedBody>(HoyolabTerminalResponse.NotFindToken(AidText));
            }
            MissionRequestBuilderFactory Factory = new MissionRequestBuilderFactory().SetHoyolabToken(Token);
            FinalizedResponse<MissionResponse> Response = await Factory.Create().SendAsync<MissionResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out MissionAnalyzedBody? AnalyzedBody))
            {
                object[] FormatArguments =
                [
                    AnalyzedBody.TotalPoint,
                    AnalyzedBody.TodayPoint - AnalyzedBody.Surplus,
                    AnalyzedBody.Mission.GetValueOrDefault(MissionType.Sign),
                    AnalyzedBody.Mission.GetValueOrDefault(MissionType.View),
                    AnalyzedBody.Mission.GetValueOrDefault(MissionType.Upvote),
                    AnalyzedBody.Mission.GetValueOrDefault(MissionType.Share),
                ];
                return TerminalResponse.Create(true, StringExtension.Format(MarkedText.HoyolabUserMissionState, FormatArguments), AnalyzedBody);
            }
            return new TerminalResponse<MissionAnalyzedBody>(false, Response.ToString());
        }
    }
}