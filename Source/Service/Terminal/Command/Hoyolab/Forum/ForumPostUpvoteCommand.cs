using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Upvote;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumPostUpvoteCommand : AsyncTerminalCommand
    {
        public override string Name => "upvote";

        public override string Help => StringExtension.Format(MarkedText.HoyolabPostUpvoteCommandHelp, '\n');

        public override async ValueTask<ITerminalResponse> AsyncInvoke(params string[] parameter)
        {
            if (!parameter.TryGetFirst(out string? PostId))
            {
                return TerminalManage.GetMissingParameterResponse();
            }
            bool IsCancel = BoolExtension.Parse(parameter.Index(1));
            string? AidText = parameter.Index(2);
            if (!HoyolabTokenManage.TryGetTokenOrFirst(AidText, out HoyolabToken? Token))
            {
                return HoyolabTerminalResponse.NotFindToken(AidText);
            }
            UpvoteRequestBuilderFactory Factory = new UpvoteRequestBuilderFactory(Token).SetPostId(PostId).SetIsCancel(IsCancel);
            FinalizedResponse<UpvoteResponse> Response = await Factory.Create().SendAsync<UpvoteResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, StringExtension.Format(MarkedText.HoyolabForumPostUpvote, PostId, !IsCancel));
            }
            return new TerminalResponse(false, Response.ToString());
        }
    }
}