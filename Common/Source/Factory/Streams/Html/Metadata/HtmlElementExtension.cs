using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Factory.Streams.Html.Metadata
{
    public static class HtmlElementExtension
    {
        [DebuggerStepThrough]
        public static HtmlElement SetAttribute(this HtmlElement element, HtmlAttribute attribute, string value)
        {
            return element.Configure(element.Attributes[attribute.GetDescription()] = value);
        }

        [DebuggerStepThrough]
        public static bool RemoveAttribute(this HtmlElement element, HtmlAttribute attribute)
        {
            return element.Attributes.Remove(attribute.GetDescription());
        }

        [DebuggerStepThrough]
        public static bool TryGetAttribute(this HtmlElement element, HtmlAttribute attribute, [NotNullWhen(true)] out string? value)
        {
            return element.Attributes.TryGetValue(attribute.GetDescription(), out value);
        }

        [DebuggerStepThrough]
        public static string? GetAttributeOrDefault(this HtmlElement element, HtmlAttribute attribute)
        {
            return element.Attributes.GetValueOrDefault(attribute.GetDescription());
        }

        [DebuggerStepThrough]
        public static string GetAttributeOrDefault(this HtmlElement element, HtmlAttribute attribute, string defaultValue)
        {
            return GetAttributeOrDefault(element, attribute) ?? defaultValue;
        }
    }
}