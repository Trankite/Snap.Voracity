using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;

namespace Common.Source.Service.Terminal.Metadata
{
    public static class SupportTerminalResponse
    {
        public static TerminalResponse UnknownOperation(string commandName)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnknownOperation.SafeFormat(commandName));
        }

        public static TerminalResponse UnlawfulParameter()
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnlawfulParameter);
        }

        public static TerminalResponse MissingUserInteraction()
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionMissingUserInteraction);
        }

        public static TerminalResponse NotFindToken(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionNotFindDefaultToken);
            }
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionNotFindToken.SafeFormat(id));
        }
    }
}