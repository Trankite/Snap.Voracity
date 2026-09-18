using System.ComponentModel;

namespace Common.Source.Factory.Streams.FileSave.Metadata
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