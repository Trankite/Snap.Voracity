using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class EnumExtension
    {
        private static readonly Dictionary<Enum, string> DescriptionCache = [];

        [DebuggerStepThrough]
        public static int ToInt<TEnum>(this TEnum value) where TEnum : Enum
        {
            return Convert.ToInt32(value);
        }

        [DebuggerStepThrough]
        public static string ToIntString<TEnum>(this TEnum value) where TEnum : Enum
        {
            return value.ToInt().ToString();
        }

        [DebuggerStepThrough]
        public static bool TryParse<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, out result) && Enum.IsDefined(result);
        }

        [DebuggerStepThrough]
        public static int GetFlags<TEnum>(this IEnumerable<TEnum> values) where TEnum : Enum
        {
            int Flags = 0;
            foreach (TEnum value in values)
            {
                Flags |= 1 << value.ToInt();
            }
            return Flags;
        }

        [DebuggerStepThrough]
        public static string GetDescription<T>(this T value) where T : struct, Enum
        {
            if (!DescriptionCache.TryGetValue(value, out string? Description))
            {
                DescriptionCache[value] = Description = typeof(T).GetDescription(Enum.GetName(value).ThrowIfNull());
            }
            return Description;
        }
    }
}