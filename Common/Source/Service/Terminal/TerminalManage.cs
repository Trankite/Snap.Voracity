using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.EHentai;
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

        static TerminalManage()
        {
            CommandTable = new ITerminalCommand[]
            {
                new FormulaCycle(),
                new QRCodeMaker(),
                new TerminalEcho(),
                new TerminalExit(),
                new TerminalHelp(),
                new TerminalInvoke(),
                new TerminalPause(),
                new EHentaiDown(),
                new EHentaiLogin(),
                new ForumNewest(),
                new ForumDetail(),
                new ForumShare(),
                new ForumUpvote(),
                new ForumSign(),
                new UserMission(),
                new UserMissionInfo(),
                new GameStamina(),
                new GameSign(),
                new DeviceFp(),
                new QRLogin(),
                new UserLogin()
            }
            .ToFrozenDictionary(Command => Command.Name, StringComparer.OrdinalIgnoreCase);
        }
    }
}