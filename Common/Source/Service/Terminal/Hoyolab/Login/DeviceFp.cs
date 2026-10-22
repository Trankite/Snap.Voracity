using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Resource.Localization;
using Common.Source.Service.Terminal.Abstract;
using Common.Source.Service.Terminal.Interface;
using Common.Source.Web.Hoyolab.Takumi.DeviceFp;
using Common.Source.Web.Request.Abstract;
using Common.Source.Web.Response;
using System.Collections.Immutable;

namespace Common.Source.Service.Terminal.Hoyolab.Login
{
    public class DeviceFp : AsyncTerminalCommand<DeviceFpResponseWrapper>
    {
        public override string Name => "devicefp";

        public override string FullName => LocalString.ServiceTerminalHoyolabLoginDeviceFpFullName;

        public override string Help => string.Empty;

        public override ImmutableArray<string> RequiredParameters { get; } = [];

        public override ImmutableArray<string> OptionalParameters { get; } = [];

        public override async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvoke(cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<ITerminalResponse<DeviceFpResponseWrapper>> AsyncInvoke(CancellationToken cancellationToken = default)
        {
            DeviceFpRequestBuilderFactory Factory = new();
            HttpResponse<DeviceFpResponse> Response = await Factory.Create().SendAsync<DeviceFpResponse>(cancellationToken).ConfigureAwait(false);
            if (Response.Body.IsNotNull() && Response.Body.TryGetDeviceFp(out string? DeviceFp))
            {
                return TerminalResponse.Create(true, DeviceFp, Response.Body.Content);
            }
            return new TerminalResponse<DeviceFpResponseWrapper>(false, Response.ToString());
        }
    }
}