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
            if (!MissionInfoResponse.Success || MissionInfoResponse.Content.IsNull() || MissionInfoResponse.Content.Surplus == 0)
            {
                return MissionInfoResponse;
            }
            Dictionary<MissionType, int> Mission = MissionInfoResponse.Content.Mission;
            for (int i = 1 - Mission.GetValueOrDefault(MissionType.Sign) - 1; i >= 0; i--)
            {
                ITerminalResponse SignResponse = await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid, cancellationToken);
                if (!SignResponse.Success)
                {
                    return TerminalResponse.Create<MissionAnalyzedBody>(SignResponse);
                }
                linkedStream?.WriteLine(SignResponse);
            }
            ITerminalResponse<NewestAnalyzedBody[]>? NewsResponse = default;
            if (Mission.ExistsKey(MissionType.View, MissionType.Upvote, MissionType.Share))
            {
                NewsResponse = await ForumNews.AsyncInvoke(5, ZoneType.StarRailWaitingRoom, default, cancellationToken);
                if (!NewsResponse.Success)
                {
                    return TerminalResponse.Create<MissionAnalyzedBody>(NewsResponse);
                }
            }
            if (NewsResponse.IsNotNull() && NewsResponse.Content.IsNotNull() && NewsResponse.Content.Length >= 5)
            {
                for (int i = 3 - Mission.GetValueOrDefault(MissionType.View, 0xff) - 1; i >= 0; i--)
                {
                    ITerminalResponse DetailResponse = await ForumDetail.AsyncInvoke(NewsResponse.Content[i].PostId, true, aid, cancellationToken);
                    if (!DetailResponse.Success)
                    {
                        return TerminalResponse.Create<MissionAnalyzedBody>(DetailResponse);
                    }
                    linkedStream?.WriteLine(DetailResponse);
                }
                for (int i = 5 - Mission.GetValueOrDefault(MissionType.Upvote, 0xff) - 1; i >= 0; i--)
                {
                    ITerminalResponse UpvoteResponse = await ForumUpvote.AsyncInvoke(NewsResponse.Content[i].PostId, false, aid, cancellationToken);
                    if (!UpvoteResponse.Success)
                    {
                        return TerminalResponse.Create<MissionAnalyzedBody>(UpvoteResponse);
                    }
                    linkedStream?.WriteLine(UpvoteResponse);
                }
                for (int i = 1 - Mission.GetValueOrDefault(MissionType.Share, 0xff) - 1; i >= 0; i--)
                {
                    ITerminalResponse ShareResponse = await ForumShare.AsyncInvoke(NewsResponse.Content[i].PostId, aid, cancellationToken);
                    if (!ShareResponse.Success)
                    {
                        return TerminalResponse.Create<MissionAnalyzedBody>(ShareResponse);
                    }
                    linkedStream?.WriteLine(ShareResponse);
                }
            }
            return await UserMissionInfo.AsyncInvoke(aid, cancellationToken);
        }
    }
}