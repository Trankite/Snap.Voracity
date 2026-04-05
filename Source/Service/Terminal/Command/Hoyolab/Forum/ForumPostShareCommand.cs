using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Share;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumPostShareCommand : AsyncTerminalCommand<ShareResponseWrapper>
    {
        public override string Name => "share";

        public override string Help => StringExtension.Format(MarkedText.HoyolabPostShareCommandHelp, '\n');

        protected override async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvokeOverride(params IList<string> parameter)
        {
            if (!parameter.TryGetFirst(out string? PostId))
            {
                return new TerminalResponse<ShareResponseWrapper>(TerminalManage.GetMissingParameterResponse());
            }
            string? AidText = parameter.Index(1);
            if (!HoyolabTokenManage.TryGetTokenOrFirst(AidText, out HoyolabToken? Token))
            {
                return new TerminalResponse<ShareResponseWrapper>(HoyolabTerminalResponse.NotFindToken(AidText));
            }
            ShareRequestBuilderFactory Factory = new ShareRequestBuilderFactory(Token).SetEntityType(EntityType.Post).SetEntityId(PostId);
            FinalizedResponse<ShareResponse> Response = await Factory.Create().SendAsync<ShareResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out ShareResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, AnalyedBody.Url, AnalyedBody);
            }
            return new TerminalResponse<ShareResponseWrapper>(false, Response.ToString());
        }
    }
}