using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Forum;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Game;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Login;
using StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Mission;
using StarRailDamage.Source.Service.Terminal.Command.Support;
using System.Collections.Frozen;

namespace StarRailDamage.Source.Service.Terminal
{
    public static class TerminalManage
    {
        public static readonly FrozenDictionary<string, ITerminalCommand> CommandTable;

        public static TerminalResponse GetUnknownOperationResponse(string commandName)
        {
            return new TerminalResponse(false, LocalString.ServiceTerminalSupportExceptionUnknownOperation.Format(commandName));
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
                new QRCodeProduce(),
                new TerminalClear(),
                new TerminalEcho(),
                new TerminalExit(),
                new TerminalHelp(),
                new TerminalInvoke(),
                new TerminalPause(),
                new ForumNews(),
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