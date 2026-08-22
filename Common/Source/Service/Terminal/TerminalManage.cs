using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Service.Terminal.Hoyolab.Forum;
using Common.Source.Service.Terminal.Hoyolab.Game;
using Common.Source.Service.Terminal.Hoyolab.Login;
using Common.Source.Service.Terminal.Hoyolab.Mission;
using Common.Source.Service.Terminal.Support;
using System.Collections.Frozen;

namespace Common.Source.Service.Terminal
{
    public static class TerminalManage
    {
        public static readonly FrozenDictionary<string, ITerminalCommand> CommandTable;

        public static TerminalResponse GetUnknownOperationResponse(string commandName)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnknownOperation.SafeFormat(commandName));
        }

        public static TerminalResponse GetUnlawfulParameterResponse()
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnlawfulParameter);
        }

        static TerminalManage()
        {
            CommandTable = new ITerminalCommand[]
            {
                new FormulaCycle(),
                new QRCodeMaker(),
                new TerminalClear(),
                new TerminalEcho(),
                new TerminalExit(),
                new TerminalHelp(),
                new TerminalInvoke(),
                new TerminalPause(),
                new ForumNewest(),
                new ForumDetail(),
                new ForumShare(),
                new ForumUpvote(),
                new ForumSign(),
                new UserMission(),
                new UserMissionInfo(),
                new GameStamina(),
                new GameSign(),
                new GameSignReward(),
                new DeviceFp(),
                new QRLogin(),
                new UserLogin()
            }
            .ToFrozenDictionary(Command => Command.Name, StringComparer.OrdinalIgnoreCase);
        }
    }
}