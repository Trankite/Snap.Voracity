using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Metadata;
using Common.Source.Web.EHentai;

namespace Common.Source.Service.Terminal.EHentai
{
    public class EHentaiLogin : AsyncTerminalCommand
    {
        public override string Name => "ehlogin";

        public override string FullName => LocalString.ServiceTerminalEHentaiEHentaiLoginFullName;

        public override string Help => LocalString.ServiceTerminalEHentaiEHentaiLoginHelp;

        public override string[] RequiredParameters => [Param_IpbMemberId, Param_IpbPassHash, Param_Igneous];

        public override string[] OptionalParameters => [];

        private const string Param_IpbMemberId = TerminalParameters.Id;

        private const string Param_IpbPassHash = "pass";

        private const string Param_Igneous = "pass";

        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = null, CancellationToken cancellationToken = default)
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
            return await AsyncInvoke(EHentaiToken, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse> AsyncInvoke(EHentaiToken eHentaiToken, CancellationToken cancellationToken = default)
        {
            try
            {
                await EHentaiTokenManage.Update(eHentaiToken, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception Exception)
            {
                return new TerminalResponse(false, Exception.Message);
            }
            return new TerminalResponse(true, LocalString.ServiceTerminalEHentaiEHentaiLoginSuccess);
        }
    }
}