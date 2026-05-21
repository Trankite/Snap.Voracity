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
            ITerminalResponse<MissionAnalyzedBody> MissionInfo = await UserMissionInfo.AsyncInvoke(aid);
            if (MissionInfo.Content.IsNull() || MissionInfo.Content.Surplus == 0)
            {
                return MissionInfo;
            }
            Dictionary<MissionType, int> Mission = MissionInfo.Content.Mission;
            for (int i = 1 - Mission.GetValueOrDefault(MissionType.Sign) - 1; i >= 0; i--)
            {
                TerminalManage.WriteLine(await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid));
            }
            ITerminalResponse<NewestAnalyzedBody[]>? ForumNews = null;
            if (Mission.ExistsKey(MissionType.View, MissionType.Upvote, MissionType.Share))
            {
                ForumNews = await Forum.ForumNews.AsyncInvoke(5, ZoneType.StarRailWaitingRoom);
            }
            if (ForumNews.IsNotNull() && ForumNews.Content.IsNotNull() && ForumNews.Content.Length >= 5)
            {
                for (int i = 3 - Mission.GetValueOrDefault(MissionType.View, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumDetail.AsyncInvoke(ForumNews.Content[i].PostId, true, aid));
                }
                for (int i = 5 - Mission.GetValueOrDefault(MissionType.Upvote, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumUpvote.AsyncInvoke(ForumNews.Content[i].PostId, false, aid));
                }
                for (int i = 1 - Mission.GetValueOrDefault(MissionType.Share, 0xff) - 1; i >= 0; i--)
                {
                    TerminalManage.WriteLine(await ForumShare.AsyncInvoke(ForumNews.Content[i].PostId, aid));
                }
            }
            return await UserMissionInfo.AsyncInvoke(aid);
        }
    }
}