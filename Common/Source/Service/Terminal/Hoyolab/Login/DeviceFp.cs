using Common.Source.Extension;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstraction;
using Common.Source.Web.Hoyolab.Takumi.DeviceFp;
using Common.Source.Web.Request;
using Common.Source.Web.Response;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class DeviceFp : AsyncTerminalCommand<DeviceFpResponseWrapper>
    {
        public override string Name => "devicefp";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginDeviceFpFullName;

        public override string Help => string.Empty;

        public override string[] RequiredParameters => [];

        public override string[] OptionalParameters => [];

        public override async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ILinkedTextStream? linkedStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(cancellationToken);
        }

        public static async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvoke(CancellationToken cancellationToken = default)
        {
            DeviceFpRequestBuilderFactory Factory = new();
            FinalizedResponse<DeviceFpResponse> Response = await Factory.Create().SendAsync<DeviceFpResponse>(cancellationToken);
            if (Response.Body.IsNotNull() && Response.Body.TryGetDeviceFp(out string? DeviceFp))
            {
                return TerminalResponse.Create(true, DeviceFp, Response.Body.Content);
            }
            return new TerminalResponse<DeviceFpResponseWrapper>(false, Response.ToString());
        }
    }
}