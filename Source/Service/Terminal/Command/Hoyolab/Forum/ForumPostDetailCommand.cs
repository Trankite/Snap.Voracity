using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.FullPost;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumPostDetailCommand : AsyncTerminalCommand<FullPostResponseWrapper>
    {
        public override string Name => "fullpost";

        public override string Help => StringExtension.Format(MarkedText.HoyolabPostDetailCommandHelp, '\n');

        protected override async ValueTask<ITerminalResponse<FullPostResponseWrapper>> AsyncInvokeOverride(params IList<string> parameter)
        {
            if (!parameter.TryGetFirst(out string? PostId))
            {
                return new TerminalResponse<FullPostResponseWrapper>(TerminalManage.GetMissingParameterResponse());
            }
            HoyolabToken? Token = null;
            string? AidText = parameter.Index(2);
            if (BoolExtension.Parse(parameter.Index(1)) && !HoyolabTokenManage.TryGetTokenOrFirst(AidText, out Token))
            {
                return new TerminalResponse<FullPostResponseWrapper>(HoyolabTerminalResponse.NotFindToken(AidText));
            }
            if (Token.IsNull())
            {
                Token = new HoyolabToken();
            }
            FullPostRequestBuilderFactory Factory = new FullPostRequestBuilderFactory(Token).SetPostId(PostId);
            FinalizedResponse<FullPostResponse> Response = await Factory.Create().SendAsync<FullPostResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out FullPostResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"[{AnalyedBody.User.Uid}] {AnalyedBody.User.Nickname}", AnalyedBody);
            }
            return new TerminalResponse<FullPostResponseWrapper>(false, Response.ToString());
        }
    }
}