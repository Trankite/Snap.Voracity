using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Service.Terminal.Hoyolab
{
    public static class HoyolabTerminalResponse
    {
        public static TerminalResponse NotFindToken(string? aid)
        {
            if (string.IsNullOrWhiteSpace(aid))
            {
                return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindDefaultToken);
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindToken.SafeFormat(aid));
        }

        public static TerminalResponse NotFindUserRole(HoyolabApp gameType)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindUserRole.SafeFormat(gameType));
        }
    }
}