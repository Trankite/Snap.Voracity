namespace Common.Source.Factory.Streams.Html.Metadata
{
    public class HtmlElement
    {
        private IList<string>? _Contents;

        private IList<HtmlElement>? _Elements;

        private Dictionary<string, string>? _Attributes;

        public string Name { get; set; } = string.Empty;

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

        public HtmlElement(string name)
        {
            Name = name;
        }

        private HtmlElement(IList<string>? contents)
        {
            _Contents = contents;
        }

        public static HtmlElement FromContent(params IList<string>? contents)
        {
            return new HtmlElement(contents);
        }

        public override string ToString()
        {
            return $"<{Name}{(Attributes.Count == 0 ? string.Empty : $"\x20{string.Join('\x20', Attributes.Select(current => $"{current.Key}=\"{current.Value}\""))}")}{(Contents.Count == 0 && Elements.Count == 0 ? "\x20/>" : $">{string.Join(string.Empty, Elements)}{string.Join(string.Empty, Contents)}</{Name}>")}";
        }
    }
}