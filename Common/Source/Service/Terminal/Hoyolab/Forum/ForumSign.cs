using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
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

        public override string[] RequiredParameters => [Param_Group];

        public override string[] OptionalParameters => [Param_Aid];

        private const string Param_Group = "group";

        private const string Param_Aid = TerminalParameters.Aid;

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke((HoyolabGroup)commandLine.GetIntParameter(Param_Group), commandLine.GetParameter(Param_Aid), cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabGroup group, string? aid = default, CancellationToken cancellationToken = default)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return SupportTerminalResponse.NotFindToken(aid);
            }
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetGroup(group);
            FinalizedResponse<SignResponse> Response = await Factory.Create().SendAsync<SignResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, LocalString.ServiceTerminalHoyolabForumSignSuccess);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}