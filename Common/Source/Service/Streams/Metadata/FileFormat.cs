using System.ComponentModel;

namespace Common.Source.Service.Streams.Metadata
{
    public enum FileFormat
    {
        None,

        Optimal,

        [Description("jpg")]
        Jpeg,

        [Description("png")]
        Png,

        [Description("bmp")]
        Bmp,

        [Description("svg")]
        Svg,

        [Description("csv")]
        Csv,

        [Description("json")]
        Json,
    }
}