namespace Common.Source.Factory.Streams.Html
{
    public class HtmlElement
    {
        public string Tag { get; set; } = string.Empty;

        public Dictionary<string, string> Attributes { get; set; } = [];

        public List<HtmlElement> Elements { get; set; } = [];

        public List<string> Contents { get; set; } = [];

        public HtmlElement() { }

        public HtmlElement(string tag)
        {
            Tag = tag;
        }

        public override string ToString()
        {
            return $"<{Tag} {string.Join('\x20', Attributes.Select(Pair => $"{Pair.Key}=\"{Pair.Value}\""))}/>";
        }
    }
}