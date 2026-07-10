namespace StarRailDamage.Source.Web.Hoyolab.Takumi.GameRole
{
    public static class GameRoleResponseWrapperExension
    {
        public static HoyolabUserRole GetUserRole(this GameRoleResponseWrapper value)
        {
            return new HoyolabUserRole() { Name = value.Nickname, Uid = value.GameUid, Game = value.GameBiz, Server = value.Region };
        }
    }
}