using System.Collections.Frozen;

namespace StarRailDamage.Source.Extension
{
    public static class FrozenDictionaryExtension
    {
        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this FrozenDictionary<TKey, TValue> frozenDictionary) where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in frozenDictionary)
            {
                yield return keyValuePair.Key;
            }
        }

        public static IEnumerable<TValue> GetValues<TKey, TValue>(this FrozenDictionary<TKey, TValue> frozenDictionary) where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in frozenDictionary)
            {
                yield return keyValuePair.Value;
            }
        }
    }
}