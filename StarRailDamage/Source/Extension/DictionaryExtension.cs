namespace StarRailDamage.Source.Extension
{
    public static class DictionaryExtension
    {
        public static bool ExistsKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params TKey[] keys) where TKey : notnull
        {
            return keys.Any(dictionary.ContainsKey);
        }

        public static bool ExistsValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params TValue[] values) where TKey : notnull
        {
            return values.Any(dictionary.ContainsValue);
        }
    }
}