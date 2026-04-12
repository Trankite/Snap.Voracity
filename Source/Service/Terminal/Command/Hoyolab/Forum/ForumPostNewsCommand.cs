using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum;
using StarRailDamage.Source.Web.Hoyolab.Bbs.Forum.Newest;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum
{
    public class ForumPostNewsCommand : AsyncTerminalCommand<NewestAnalyzedBody[]>
    {
        public override string Name => "newpost";

        public override string Help => MarkedText.HoyolabPostNewsCommandHelp;

        protected override async ValueTask<ITerminalResponse<NewestAnalyzedBody[]>> AsyncInvokeOverride(params IList<string> parameter)
        {
            int PageSize = IntExtension.Parse(parameter.FirstOrDefault());
            ZoneType ZoneType = (ZoneType)IntExtension.Parse(parameter.Index(1));
            SortType SortType = (SortType)IntExtension.Parse(parameter.Index(2));
            NewestRequestBuilderFactory Factory = new NewestRequestBuilderFactory().SetPageSize(PageSize).SetZoneType(ZoneType).SetSortType(SortType);
            FinalizedResponse<NewestResponse> Response = await Factory.Create().SendAsync<NewestResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetAnalyzedBody(out NewestAnalyzedBody[]? AnalyzedBody))
            {
                if (AnalyzedBody.Length == 0)
                {
                    return new TerminalResponse<NewestAnalyzedBody[]>(TerminalManage.GetInvalidParameterResponse());
                }
                return TerminalResponse.Create(true, string.Join('\n', AnalyzedBody.Select(Body => $"[{Body.PostId}] {Body.Title}")), AnalyzedBody);
            }
            return new TerminalResponse<NewestAnalyzedBody[]>(false, Response.ToString());
        }
    }
}