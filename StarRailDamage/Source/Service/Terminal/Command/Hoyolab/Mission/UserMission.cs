using StarRailDamage.Source.Core.Abstraction;
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

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [AID];

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID), this);
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = null, ILinkedTextStream? stream = null)
        {
            ITerminalResponse<MissionAnalyzedBody> MissionInfo = await UserMissionInfo.AsyncInvoke(aid);
            if (MissionInfo.Content.IsNull() || MissionInfo.Content.Surplus == 0)
            {
                return MissionInfo;
            }
            Dictionary<MissionType, int> Mission = MissionInfo.Content.Mission;
            for (int i = 1 - Mission.GetValueOrDefault(MissionType.Sign) - 1; i >= 0; i--)
            {
                (await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid)).Configure(Self => stream?.WriteLine(Self));
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
                    (await ForumDetail.AsyncInvoke(ForumNews.Content[i].PostId, true, aid)).Configure(Self => stream?.WriteLine(Self));
                }
                for (int i = 5 - Mission.GetValueOrDefault(MissionType.Upvote, 0xff) - 1; i >= 0; i--)
                {
                    (await ForumUpvote.AsyncInvoke(ForumNews.Content[i].PostId, false, aid)).Configure(Self => stream?.WriteLine(Self));
                }
                for (int i = 1 - Mission.GetValueOrDefault(MissionType.Share, 0xff) - 1; i >= 0; i--)
                {
                    (await ForumShare.AsyncInvoke(ForumNews.Content[i].PostId, aid)).Configure(Self => stream?.WriteLine(Self));
                }
            }
            return await UserMissionInfo.AsyncInvoke(aid);
        }
    }
}