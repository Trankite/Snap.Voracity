using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Sign;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumSign : AsyncTerminalCommand
    {
        public override string Name => "fsign";

        public override string Help => LocalString.ServiceTerminalHoyolabForumSignHelp;

        public override string[] Parameters => [GROUP, AID];

        private const string GROUP = "group";

        private const string AID = "aid";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            if (!commandLine.TryGetEnumParameter(GROUP, out HoyolabGroup Group))
            {
                return TerminalManage.GetUnlawfulParameterResponse();
            }
            return await AsyncInvoke(Group, commandLine.GetParameter(AID));
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(HoyolabGroup group, string? aid = null)
        {
            if (!HoyolabTokenManage.TryGetTokenOrFirst(aid, out HoyolabToken? Token))
            {
                return HoyolabTerminalResponse.NotFindToken(aid);
            }
            SignRequestBuilderFactory Factory = new SignRequestBuilderFactory(Token).SetGroup(group);
            FinalizedResponse<SignResponse> Response = await Factory.Create().SendAsync<SignResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.IsSuccess())
            {
                return new TerminalResponse(true, LocalString.WebHoyolabForumSignSuccess);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}