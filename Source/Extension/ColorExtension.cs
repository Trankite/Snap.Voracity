using System.Drawing;

namespace StarRailDamage.Source.Extension
{
    public static class ColorExtension
    {
        public static bool TryFromHtml(string? value, out Color color)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try { return (color = ColorTranslator.FromHtml(value)) != Color.Empty; } catch { }
            }
            return false.Configure(color = default);
        }
    }
}