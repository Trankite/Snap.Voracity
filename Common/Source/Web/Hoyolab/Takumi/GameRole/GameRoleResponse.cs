using Common.Source.Factory.Serialize.Interface;
using Common.Source.Web.Response;
using Common.Source.Web.Response.Wrapper;

namespace Common.Source.Web.Hoyolab.Takumi.GameRole
{
    public class GameRoleResponse : ResponseWrapper<ListWrapper<GameRoleResponseWrapper>>, IContentSerializer<IEnumerable<HoyolabUserRole>?>
    {
        public IEnumerable<HoyolabUserRole>? Serialize()
        {
            return TryGetContent(out ListWrapper<GameRoleResponseWrapper>? Content) ? Content.List.Select(GetUserRole) : default;
        }

        public static HoyolabUserRole GetUserRole(GameRoleResponseWrapper response)
        {
            return new HoyolabUserRole() { Name = response.Nickname, Uid = response.GameUid, Game = response.GameBiz, Server = response.Region };
        }
    }
}