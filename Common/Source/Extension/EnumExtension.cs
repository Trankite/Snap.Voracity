using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class EnumExtension
    {
        private static readonly Dictionary<Enum, string> DescriptionCache;

        [DebuggerStepThrough]
        public static TEnum Parse<TEnum>(string? value) where TEnum : struct, Enum
        {
            return TryParse(value, out TEnum result) ? result : default;
        }

        [DebuggerStepThrough]
        public static bool TryParse<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, out result) && Enum.IsDefined(result);
        }

        [DebuggerStepThrough]
        public static string GetIntString<TEnum>(this TEnum value) where TEnum : Enum
        {
            return Convert.ToInt32(value).ToString();
        }

        [DebuggerStepThrough]
        public static int GetFlags<TEnum>(params ReadOnlySpan<TEnum> values) where TEnum : Enum
        {
            int flags = 0;
            foreach (TEnum value in values)
            {
                flags |= 1 << Convert.ToInt32(value);
            }
            return flags;
        }

        [DebuggerStepThrough]
        public static string GetDescription<T>(this T value) where T : struct, Enum
        {
            if (DescriptionCache.TryGetValue(value, out string? description))
            {
                return description;
            }
            return DescriptionCache[value] = typeof(T).GetDescription(Enum.GetName(value).ThrowIfNull());
        }

        private static int EnumGetHashCode(Enum sender)
        {
            return HashCode.Combine(sender.GetType().TypeHandle.Value, sender.GetHashCode());
        }

        static EnumExtension()
        {
            DescriptionCache = new Dictionary<Enum, string>(EqualityComparer<Enum>.Create(Equals, EnumGetHashCode));
        }
    }
}