namespace Common.Source.Factory.Streams.Html
{
    public class HtmlElement
    {
        private IList<string>? _Contents;

        private IList<HtmlElement>? _Elements;

        private Dictionary<string, string>? _Attributes;

        public string Markup { get; set; } = string.Empty;

        public IList<string> Contents
        {
            get => _Contents ??= [];
            set => _Contents = value;
        }

        public IList<HtmlElement> Elements
        {
            get => _Elements ??= [];
            set => _Elements = value;
        }

        public Dictionary<string, string> Attributes
        {
            get => _Attributes ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _Attributes = value;
        }

        public HtmlElement() { }

        public HtmlElement(string markup)
        {
            Markup = markup;
        }

        public override string ToString()
        {
            return $"<{Markup} {string.Join('\x20', Attributes.Select(Current => $"{Current.Key}=\"{Current.Value}\""))} />";
        }
    }
}