using Common.Source.Extension;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Factory.Streams.Html
{
    public static class HtmlElementExtension
    {
        public static HtmlElement SetAttribute(this HtmlElement element, HtmlAttribute attribute, string value)
        {
            return element.Configure(element.Attributes[attribute.GetDescription()] = value);
        }

        public static bool RemoveAttribute(this HtmlElement element, HtmlAttribute attribute)
        {
            return element.Attributes.Remove(attribute.GetDescription());
        }

        public static bool TryGetAttribute(this HtmlElement element, HtmlAttribute attribute, [NotNullWhen(true)] out string? value)
        {
            return element.Attributes.TryGetValue(attribute.GetDescription(), out value);
        }

        public static string? GetAttributeOrDefault(this HtmlElement element, HtmlAttribute attribute)
        {
            return element.Attributes.GetValueOrDefault(attribute.GetDescription());
        }
    }
}