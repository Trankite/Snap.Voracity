using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Resource.Localization;
using StarRailDamage.Source.Service.Terminal.Abstraction;
using StarRailDamage.Source.Web.Hoyolab.Takumi.DeviceFp;
using StarRailDamage.Source.Web.Request;
using StarRailDamage.Source.Web.Response;

namespace StarRailDamage.Source.Service.Terminal.Command.Hoyolab.Login
{
    public class DeviceFp : AsyncTerminalCommand<DeviceFpResponseWrapper>
    {
        public override string Name => "devicefp";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginDeviceFpFullName;

        public override string Help => string.Empty;

        public override string[] Parameters => [];

        protected override async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine)
        {
            return await AsyncInvoke();
        }

        public static async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvoke()
        {
            DeviceFpRequestBuilderFactory Factory = new();
            FinalizedResponse<DeviceFpResponse> Response = await Factory.Create().SendAsync<DeviceFpResponse>(Program.HttpClient);
            if (Response.Body.IsNotNull() && Response.Body.TryGetDeviceFp(out string? DeviceFp))
            {
                return TerminalResponse.Create(true, DeviceFp, Response.Body.Content);
            }
            return new TerminalResponse<DeviceFpResponseWrapper>(false, Response.ToString());
        }
    }
}