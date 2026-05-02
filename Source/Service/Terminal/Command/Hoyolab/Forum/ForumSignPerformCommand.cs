using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Sign;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumSignPerformCommand : AsyncTerminalCommand
    {
        public override string Name => "fsign";

        public override string Help => MarkedText.HoyolabForumSignCommandHelp;

        public override async ValueTask<ITerminalResponse> AsyncInvoke(IList<string> parameter)
        {
            if (!EnumExtension.TryParse(parameter.Index(0), out HoyolabGroup Group))
            {
                return TerminalManage.GetInvalidParameterResponse();
            }
            return await AsyncInvoke(Group, parameter.Index(1));
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
                return new TerminalResponse(true, MarkedText.HoyolabForumSign);
            }
            return new TerminalResponse<SignResponseWrapper>(false, Response.ToString());
        }
    }
}