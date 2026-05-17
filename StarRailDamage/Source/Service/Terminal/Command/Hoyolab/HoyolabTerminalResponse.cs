using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Web.Hoyolab;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab
{
    public static class HoyolabTerminalResponse
    {
        public static TerminalResponse NotFindToken(string? aid)
        {
            if (string.IsNullOrWhiteSpace(aid))
            {
                return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindDefaultToken);
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindToken.Format(aid));
        }

        public static TerminalResponse NotFindUserRole(GameType gameType)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindUserRole.Format(gameType));
        }
    }
}