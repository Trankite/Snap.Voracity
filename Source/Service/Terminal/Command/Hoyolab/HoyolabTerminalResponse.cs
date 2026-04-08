using StarRailDamage.Source.Core.LocalText.Marked.Text;
using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Web.Hoyolab;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab
{
    public static class HoyolabTerminalResponse
    {
        public static TerminalResponse NotFindToken(string? aid)
        {
            if (string.IsNullOrWhiteSpace(aid))
            {
                return new TerminalResponse(false, MarkedText.HoyolabNotFindDefaultToken);
            }
            return new TerminalResponse(false, StringExtension.Format(MarkedText.HoyolabNotFindToken, aid));
        }

        public static TerminalResponse NotFindUserRole(GameType gameType)
        {
            return new TerminalResponse(false, StringExtension.Format(MarkedText.HoyolabNotFindUserRole, gameType));
        }
    }
}