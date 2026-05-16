using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
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

        public override string Help => MarkedText.HoyolabPostShareCommandHelp;

        public override string[] Parameters => [POSTID, AID];

        private const string POSTID = "id";

        private const string AID = "aid";

        protected override async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetParameter(POSTID, out string? PostId))
            {
                return new TerminalResponse<ShareResponseWrapper>(TerminalManage.GetMissingParameterResponse());
            }
            return await AsyncInvoke(PostId, commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse<ShareResponseWrapper>> AsyncInvoke(string postId, string? aid = null)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return new TerminalResponse<ShareResponseWrapper>(HoyolabTerminalResponse.NotFindToken(aid));
            }
            ShareRequestBuilderFactory Factory = new ShareRequestBuilderFactory(Token).SetEntityType(EntityType.Post).SetEntityId(postId);
            FinalizedResponse<ShareResponse> Response = await Factory.Create().SendAsync<ShareResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out ShareResponseWrapper? AnalyedBody))
            {
                return TerminalResponse.Create(true, $"{AnalyedBody.Title}\n{AnalyedBody.Url}", AnalyedBody);
            }
            return new TerminalResponse<ShareResponseWrapper>(false, Response.ToString());
        }
    }
}