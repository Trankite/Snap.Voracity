using Common.Source.Extension;
using Common.Source.Web.Hoyolab.DataSign;
using Common.Source.Web.Hoyolab.Metadata;
using Common.Source.Web.Request.Builder.Abstract;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Builder
{
    public static class HoyolabHttpRequestMessageBuilderExtension
    {
        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetReferer(this HoyolabHttpRequestMessageBuilder builder, HoyolabReferer value)
        {
            return builder.SetHeader("Referer", value.GetDescription());
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetXrpcAppId(this HoyolabHttpRequestMessageBuilder builder, string value)
        {
            return builder.SetHeader("x-rpc-app_id", value);
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetXrpcAppVersion(this HoyolabHttpRequestMessageBuilder builder, string value)
        {
            return builder.SetHeader("x-rpc-app_version", value);
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetXrpcDeviceId(this HoyolabHttpRequestMessageBuilder builder, string value)
        {
            return builder.SetHeader("x-rpc-device_id", value);
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetXrpcClientType(this HoyolabHttpRequestMessageBuilder builder, HoyolabClient clientType)
        {
            return builder.SetHeader("x-rpc-client_type", clientType.GetIntString());
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetXrpcDeviceFp(this HoyolabHttpRequestMessageBuilder builder, string value)
        {
            return builder.SetHeader("x-rpc-device_fp", value);
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetDataSign(this HoyolabHttpRequestMessageBuilder builder, DataSignOptions dataSignOptions)
        {
            return builder.SetHeader("DS", dataSignOptions.GetDataSign());
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetDataSignWithBody(this HoyolabHttpRequestMessageBuilder builder, DataSignOptions dataSignOptions, string body)
        {
            return builder.SetStringContent(body).SetDataSign(dataSignOptions.SetDataSignBody(body));
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetDataSignWithQuery(this HoyolabHttpRequestMessageBuilder builder, DataSignOptions dataSignOptions)
        {
            return builder.SetDataSign(dataSignOptions.SetDataSignQuery(builder));
        }

        [DebuggerStepThrough]
        public static HoyolabHttpRequestMessageBuilder SetDataSignWithBodyAndQuery(this HoyolabHttpRequestMessageBuilder builder, DataSignOptions dataSignOptions, string body)
        {
            return builder.SetStringContent(body).SetDataSign(dataSignOptions.SetDataSignBody(body).SetDataSignQuery(builder));
        }

        private static DataSignOptions SetDataSignBody(this DataSignOptions dataSignOptions, string? body)
        {
            return dataSignOptions.Configure(dataSignOptions.Body = body);
        }

        private static DataSignOptions SetDataSignQuery(this DataSignOptions dataSignOptions, HoyolabHttpRequestMessageBuilder builder)
        {
            return dataSignOptions.Configure(dataSignOptions.Query = builder.RequestUri?.Query);
        }
    }
}