using Common.Source.Web.Response;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Web.Hoyolab.Takumi.DeviceFp
{
    public class DeviceFpResponse : ResponseWrapper<DeviceFpResponseWrapper>
    {
        public bool TryGetDeviceFp([NotNullWhen(true)] out string? deviceFp)
        {
            return !string.IsNullOrEmpty(deviceFp = Content?.DeviceFp);
        }
    }
}