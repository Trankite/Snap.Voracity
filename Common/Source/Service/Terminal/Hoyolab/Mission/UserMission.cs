using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Hoyolab.Forum;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.Hoyolab.Bbs.Forum;
using Common.Source.Web.Hoyolab.Bbs.Forum.Newest;
using Common.Source.Web.Hoyolab.Bbs.Mission;
using Common.Source.Web.Hoyolab.Metadata;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Mission
{
    public class UserMission : AsyncTerminalCommand<MissionInfo>
    {
        public override string Name => "coin";

        public override string FullName => LocalString.ServiceTerminalHoyolabUserMissionFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabUserMissionHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [Param_Aid];

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(commandLine.GetParameter(Param_Aid), textStream, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<MissionInfo>> AsyncInvoke(string? aid = default, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            ITerminalResponse<MissionInfo> missionInfoResponse = await UserMissionInfo.AsyncInvoke(aid, cancellationToken).ConfigureAwait(false);
            if (!missionInfoResponse.TryGetContent(out MissionInfo? missionInfo) || missionInfo.Surplus == 0)
            {
                return missionInfoResponse;
            }
            bool isUpdated = false;
            bool VerifyMissionStatus(ITerminalResponse response)
            {
                isUpdated |= response.Success;
                textStream?.WriteLine(response);
                return response.Success;
            }
            Dictionary<MissionType, int> mission = missionInfo.Mission;
            for (int i = mission.GetValueOrDefault(MissionType.Sign); i < 1; i++)
            {
                if (!VerifyMissionStatus(await ForumSign.AsyncInvoke(HoyolabGroup.StarRail, aid, cancellationToken).ConfigureAwait(false)))
                {
                    break;
                }
            }

            // https://www.miyoushe.com/ys/article/74971515
            // 2026-04-28 已正式下线米游币任务「每日任务」中的“浏览帖子”、“点赞帖子”与“分享帖子”任务
            const bool MissionViewUpvoteShareEnabled = false;
            if (MissionViewUpvoteShareEnabled && mission.ExistsKey(MissionType.View, MissionType.Upvote, MissionType.Share))
            {
                ITerminalResponse<IEnumerable<NewestPost>> newestResponse = await ForumNewest.AsyncInvoke(5, ZoneType.StarRailWaitingRoom, default, cancellationToken).ConfigureAwait(false);
                if (!newestResponse.TryGetContent(out IEnumerable<NewestPost>? newestEnumerable) || newestEnumerable.ToArray().OutSender(out NewestPost[]? newestPosts).Length < 5)
                {
                    return TerminalResponse.Create<MissionInfo>(newestResponse);
                }
                for (int i = mission.GetValueOrDefault(MissionType.View); i < 3; i++)
                {
                    if (!VerifyMissionStatus(await ForumDetail.AsyncInvoke(newestPosts[i].PostId, true, aid, cancellationToken).ConfigureAwait(false)))
                    {
                        break;
                    }
                }
                for (int i = mission.GetValueOrDefault(MissionType.Upvote); i < 5; i++)
                {
                    if (!VerifyMissionStatus(await ForumUpvote.AsyncInvoke(newestPosts[i].PostId, false, aid, cancellationToken).ConfigureAwait(false)))
                    {
                        break;
                    }
                }
                for (int i = mission.GetValueOrDefault(MissionType.Share); i < 1; i++)
                {
                    if (!VerifyMissionStatus(await ForumShare.AsyncInvoke(newestPosts[i].PostId, aid, cancellationToken).ConfigureAwait(false)))
                    {
                        break;
                    }
                }
            }

            return isUpdated ? await UserMissionInfo.AsyncInvoke(aid, cancellationToken).ConfigureAwait(false) : missionInfoResponse;
        }
    }
}