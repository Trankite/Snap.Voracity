using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Hoyolab.Forum;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab.Bbs.Forum;
using Common.Source.Web.Hoyolab.Bbs.Forum.Newest;
using Common.Source.Web.Hoyolab.Bbs.Mission;
using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMission : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "coin";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionHelp;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), linkedStream, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvoke(string? aid = default, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            ITerminalResponse<MissionAnalyzedBody> MissionInfoResponse = await UserMissionInfo.AsyncInvoke(aid, cancellationToken).ConfigureAwait(false);
            if (!MissionInfoResponse.TryGetAnalyzedBody(out MissionAnalyzedBody? MissionInfoContent) || MissionInfoContent.Surplus == 0)
            {
                return MissionInfoResponse;
            }
            bool IsUpdated = false;
            bool VerifyMissionStatus(ITerminalResponse response)
            {
                linkedStream?.WriteLine(response);
                response.Success.OverlayIfTrue(ref IsUpdated);
                return response.Success;
            }
            Dictionary<MissionType, int> Mission = MissionInfoContent.Mission;
            for (int i = 1 - Mission.GetValueOrDefault(MissionType.Sign) - 1; i >= 0; i--)
            {
                if (!VerifyMissionStatus(await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid, cancellationToken).ConfigureAwait(false))) break;
            }

            // https://www.miyoushe.com/ys/article/74971515
            // 2026-04-28 已正式下线米游币任务「每日任务」中的“浏览帖子”、“点赞帖子”与“分享帖子”任务
            const bool MissionViewUpvoteShareEnabled = false;
            if (MissionViewUpvoteShareEnabled && Mission.ExistsKey(MissionType.View, MissionType.Upvote, MissionType.Share))
            {
                ITerminalResponse<NewestAnalyzedBody[]>? NewestResponse = await ForumNewest.AsyncInvoke(5, ZoneType.StarRailWaitingRoom, default, cancellationToken).ConfigureAwait(false);
                if (!NewestResponse.TryGetAnalyzedBody(out NewestAnalyzedBody[]? NewsContent) || NewsContent.Length < 5)
                {
                    return TerminalResponse.Create<MissionAnalyzedBody>(NewestResponse);
                }
                for (int i = 3 - Mission.GetValueOrDefault(MissionType.View) - 1; i >= 0; i--)
                {
                    if (!VerifyMissionStatus(await ForumDetail.AsyncInvoke(NewsContent[i].PostId, true, aid, cancellationToken).ConfigureAwait(false))) break;
                }
                for (int i = 5 - Mission.GetValueOrDefault(MissionType.Upvote) - 1; i >= 0; i--)
                {
                    if (!VerifyMissionStatus(await ForumUpvote.AsyncInvoke(NewsContent[i].PostId, false, aid, cancellationToken).ConfigureAwait(false))) break;
                }
                for (int i = 1 - Mission.GetValueOrDefault(MissionType.Share) - 1; i >= 0; i--)
                {
                    if (!VerifyMissionStatus(await ForumShare.AsyncInvoke(NewsContent[i].PostId, aid, cancellationToken).ConfigureAwait(false))) break;
                }
            }

            return IsUpdated ? await UserMissionInfo.AsyncInvoke(aid, cancellationToken).ConfigureAwait(false) : MissionInfoResponse;
        }
    }
}