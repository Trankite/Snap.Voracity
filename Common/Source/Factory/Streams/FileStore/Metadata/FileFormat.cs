using System.ComponentModel;

namespace Common.Source.Factory.Streams.FileStore.Metadata
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