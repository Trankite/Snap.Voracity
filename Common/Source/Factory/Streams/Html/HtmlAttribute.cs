using System.ComponentModel;

namespace Common.Source.Factory.Streams.Html
{
    public enum HtmlAttribute
    {
        None,

        [Description("id")]
        Id,

        [Description("class")]
        Class,

        [Description("style")]
        Style,

        [Description("title")]
        Title,

        [Description("src")]
        Source,

        [Description("lang")]
        Language,

        [Description("tabindex")]
        TabIndex,

        [Description("hidden")]
        Hidden,

        [Description("href")]
        Href
    }
}