using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Web.Hoyolab.Metadata;

namespace Common.Source.Service.Terminal.Metadata
{
    public static class HoyolabTerminalResponse
    {
        public static TerminalResponse NotFindUserRole(HoyolabApp gameType)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalHoyolabExceptionNotFindUserRole.SafeFormat(gameType));
        }
    }
}