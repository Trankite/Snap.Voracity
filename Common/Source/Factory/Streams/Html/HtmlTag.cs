using System.ComponentModel;

namespace Common.Source.Factory.Streams.Html
{
    public enum HtmlTag
    {
        None,

        [Description("html")]
        Html,

        [Description("head")]
        Head,

        [Description("title")]
        Title,

        [Description("body")]
        Body,

        [Description("script")]
        Script,

        [Description("style")]
        Style,

        [Description("div")]
        Division‌,

        [Description("span")]
        Span,

        [Description("table")]
        Table,

        [Description("tr")]
        TableRow,

        [Description("td")]
        TableData,

        [Description("map")]
        ImageMap,

        [Description("area")]
        Area,

        [Description("img")]
        Image,

        [Description("p")]
        Paragraph,

        [Description("a")]
        Anchor
    }
}