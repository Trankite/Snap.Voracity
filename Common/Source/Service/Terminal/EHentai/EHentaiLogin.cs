using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.EHentai;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.EHentai
{
    public class EHentaiLogin : TerminalCommand
    {
        public override string Name => "ehlogin";

        public override string FullName => LocalString.ServiceTerminalEHentaiEHentaiLoginFullName;

        public override string Help => LocalString.ServiceTerminalEHentaiEHentaiLoginHelp;

        public override ImmutableArray<string> RequiredParameters { get; } = [Param_IpbMemberId, Param_IpbPassHash, Param_Igneous];

        public override ImmutableArray<string> OptionalParameters { get; } = [];

        private const string Param_IpbMemberId = TerminalParameters.Id;

        private const string Param_IpbPassHash = "pass";

        private const string Param_Igneous = "pass";

        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = null, CancellationToken cancellationToken = default)
        {
            if (!commandLine.TryGetParameter(Param_IpbMemberId, out string? IpbMemberId))
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            if (!commandLine.TryGetParameter(Param_IpbPassHash, out string? IpbPassHash))
            {
                return SupportTerminalResponse.UnlawfulParameter();
            }
            EHentaiToken EHentaiToken = new EHentaiToken(IpbMemberId).SetToken(IpbPassHash);
            EHentaiToken.Igneous = commandLine.GetParameter(Param_Igneous);
            return Invoke(EHentaiToken);
        }

        public static ITerminalResponse Invoke(EHentaiToken eHentaiToken)
        {
            try
            {
                EHentaiTokenManage.Update(eHentaiToken);
            }
            catch (Exception Exception)
            {
                return new TerminalResponse(false, Exception.Message);
            }
            return new TerminalResponse(true, LocalString.ServiceTerminalEHentaiEHentaiLoginSuccess);
        }
    }
}