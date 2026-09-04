using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab;
using Common.Source.Web.Hoyolab.Bbs.Sign;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Forum
{
    public class ForumSign : AsyncTerminalCommand
    {
        public override string Name => "fsign";

        public override string FullName => LocalString.ServiceTerminalHoyolabForumSignFullName;

        public override string Help => LocalString.ServiceTerminalHoyolabForumSignHelp;

        public override string[] RequiredParameters => [GROUP];

        public override string[] OptionalParameters => [AID];

        private const string GROUP = "group";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke((HoyolabGroup)commandLine.GetIntParameter(GROUP), commandLine.GetParameter(AID), cancellationToken);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabGroup group, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return HoyolabTerminalResponse.NotFindToken(aid);
            }
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetGroup(group);
            FinalizedResponse<SignResponse> Response = await Factory.Create().SendAsync<SignResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, LocalString.ServiceTerminalHoyolabForumSignSuccess);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}