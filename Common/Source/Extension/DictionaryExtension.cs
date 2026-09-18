using System.Collections.Frozen;
using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class DictionaryExtension
    {
        [DebuggerStepThrough]
        public static bool ExistsKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params TKey[] keys) where TKey : notnull
        {
            return keys.Any(dictionary.ContainsKey);
        }

        [DebuggerStepThrough]
        public static bool ExistsValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, params TValue[] values) where TKey : notnull
        {
            return values.Any(dictionary.ContainsValue);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this FrozenDictionary<TKey, TValue> frozenDictionary) where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in frozenDictionary)
            {
                yield return keyValuePair.Key;
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<TValue> GetValues<TKey, TValue>(this FrozenDictionary<TKey, TValue> frozenDictionary) where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in frozenDictionary)
            {
                yield return keyValuePair.Value;
            }
        }
    }
}