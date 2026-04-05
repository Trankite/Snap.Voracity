using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Mission;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission
{
    public class UserMissionPerformCommand : AsyncTerminalCommand<MissionAnalyzedBody>
    {
        public override string Name => "coin";

        public override string Help => StringExtension.Format(MarkedText.HoyolabUserMissionPerformCommandHelp, '\n');

        protected override async ValueTask<ITerminalResponse<MissionAnalyzedBody>> AsyncInvokeOverride(params IList<string> parameter)
        {
            string AidText = parameter.FirstOrDefault() ?? string.Empty;
            IAsyncTerminalCommand<MissionAnalyzedBody> MissionStateCommand = new UserMissionStateCommand();
            ITerminalResponse<MissionAnalyzedBody> MissionState = await MissionStateCommand.AsyncInvoke(AidText);
            if (MissionState.Content.IsNull() || MissionState.Content.Surplus == 0)
            {
                return MissionState;
            }
            if (MissionState.Content.Mission.GetValueOrDefault(MissionType.Sign) < 1)
            {
                TerminalHelper.WriteLine(await new ForumSignPerformCommand().AsyncInvoke(HoyolabGroup.StarRail.ToIntString(), AidText));
            }
            IAsyncTerminalCommand<NewestAnalyzedBody[]> PostNewsCommand = new ForumPostNewsCommand();
            ITerminalResponse<NewestAnalyzedBody[]> PostNews = await PostNewsCommand.AsyncInvoke(5.ToString(), ZoneType.StarRailWaitingRoom.ToIntString());
            if (PostNews.Content.IsNotNull() && PostNews.Content.Length >= 5)
            {
                for (int i = 3 - MissionState.Content.Mission.GetValueOrDefault(MissionType.View) - 1; i >= 0; i--)
                {
                    TerminalHelper.WriteLine(await new ForumPostDetailCommand().AsyncInvoke(PostNews.Content[i].PostId, true.ToString(), AidText));
                }
                for (int i = 5 - MissionState.Content.Mission.GetValueOrDefault(MissionType.Upvote) - 1; i >= 0; i--)
                {
                    TerminalHelper.WriteLine(await new ForumPostUpvoteCommand().AsyncInvoke(PostNews.Content[i].PostId, false.ToString(), AidText));
                }
                for (int i = 1 - MissionState.Content.Mission.GetValueOrDefault(MissionType.Share) - 1; i >= 0; i--)
                {
                    TerminalHelper.WriteLine(await new ForumPostShareCommand().AsyncInvoke(PostNews.Content[i].PostId, AidText));
                }
            }
            return await MissionStateCommand.AsyncInvoke(AidText);
        }
    }
}