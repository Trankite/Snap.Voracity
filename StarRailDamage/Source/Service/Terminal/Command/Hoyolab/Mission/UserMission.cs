using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Mission;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission
{
    public class UserMission : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "coin";

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionHelp;

        public override string[] Parameters => [AID];

        private const string AID = "aid";

        protected override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = null)
        {
            ITerminalResponse<MissionAnalyzedBody> MissionState = await UserMissionInfo.AsyncInvoke(aid);
            if (MissionState.Content.IsNull() || MissionState.Content.Surplus == 0)
            {
                return MissionState;
            }
            for (int i = 1 - MissionState.Content.Mission.GetValueOrDefault(MissionType.Sign) - 1; i >= 0; i--)
            {
                TerminalManage.WriteLine(await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid));
            }
            ITerminalResponse<NewestAnalyzedBody[]> PostNews = await ForumNews.AsyncInvoke(5, ZoneType.StarRailWaitingRoom);
            if (PostNews.Content.IsNotNull() && PostNews.Content.Length >= 5)
            {
                for (int i = 3 - MissionState.Content.Mission.GetValueOrDefault(MissionType.View, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumDetail.AsyncInvoke(PostNews.Content[i].PostId, true, aid));
                }
                for (int i = 5 - MissionState.Content.Mission.GetValueOrDefault(MissionType.Upvote, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumUpvote.AsyncInvoke(PostNews.Content[i].PostId, false, aid));
                }
                for (int i = 1 - MissionState.Content.Mission.GetValueOrDefault(MissionType.Share, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumShare.AsyncInvoke(PostNews.Content[i].PostId, aid));
                }
            }
            return await UserMissionInfo.AsyncInvoke(aid);
        }
    }
}