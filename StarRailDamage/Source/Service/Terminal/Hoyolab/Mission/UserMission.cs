using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Hoyolab.Forum;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Mission;

namespace StarRailDamage.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMission : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "coin";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [AID];

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(AID), linkedStream, cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = default, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            ITerminalResponse<MissionAnalyzedBody> MissionInfoResponse = await UserMissionInfo.AsyncInvoke(aid, cancellationToken);
            if (!MissionInfoResponse.TryGetAnalyzedBody(out MissionAnalyzedBody? MissionInfoContent) || MissionInfoContent.Surplus == 0)
            {
                return MissionInfoResponse;
            }
            Dictionary<MissionType, int> Mission = MissionInfoContent.Mission;
            for (int i = 1 - Mission.GetValueOrDefault(MissionType.Sign, 0xff); i >= 0; i--)
            {
                ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid, cancellationToken).AsTask().Result.Configure(Self => linkedStream?.WriteLine(Self));
            }
            if (Mission.ExistsKey(MissionType.View, MissionType.Upvote, MissionType.Share))
            {
                ITerminalResponse<NewestAnalyzedBody[]>? NewsResponse = await ForumNews.AsyncInvoke(5, ZoneType.StarRailWaitingRoom, default, cancellationToken);
                if (!NewsResponse.TryGetAnalyzedBody(out NewestAnalyzedBody[]? NewsContent) || NewsContent.Length < 5)
                {
                    return TerminalResponse.Create<MissionAnalyzedBody>(NewsResponse);
                }
                for (int i = 3 - Mission.GetValueOrDefault(MissionType.View, 0xff) - 1; i >= 0; i--)
                {
                    ForumDetail.AsyncInvoke(NewsContent[i].PostId, true, aid, cancellationToken).AsTask().Result.Configure(Self => linkedStream?.WriteLine(Self));
                }
                for (int i = 5 - Mission.GetValueOrDefault(MissionType.Upvote, 0xff) - 1; i >= 0; i--)
                {
                    ForumUpvote.AsyncInvoke(NewsContent[i].PostId, false, aid, cancellationToken).AsTask().Result.Configure(Self => linkedStream?.WriteLine(Self));
                }
                for (int i = 1 - Mission.GetValueOrDefault(MissionType.Share, 0xff) - 1; i >= 0; i--)
                {
                    ForumShare.AsyncInvoke(NewsContent[i].PostId, aid, cancellationToken).AsTask().Result.Configure(Self => linkedStream?.WriteLine(Self));
                }
            }
            return await UserMissionInfo.AsyncInvoke(aid, cancellationToken);
        }
    }
}