namespace Common.Source.Factory.Streams.Html
{
    public class HtmlElement
    {
        public string Markup { get; set; } = string.Empty;

        public Dictionary<string, string> Attributes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public IList<HtmlElement> Elements { get; set; } = [];

        public IList<string> Contents { get; set; } = [];

        public HtmlElement() { }

        public HtmlElement(string markup)
        {
            Markup = markup;
        }

        public override string ToString()
        {
            return $"<{Markup} {string.Join('\x20', Attributes.Select(Attribute => $"{Attribute.Key}=\"{Attribute.Value}\""))} />";
        }
    }
}