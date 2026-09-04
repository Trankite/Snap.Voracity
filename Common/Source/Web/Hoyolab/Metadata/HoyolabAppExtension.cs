using Common.Source.Extension;
using System.Collections.Frozen;

namespace Common.Source.Web.Hoyolab.Metadata
{
    public static class HoyolabAppExtension
    {
        private static readonly FrozenDictionary<string, HoyolabApp> GameTypeTable;

        public static bool TryGetValue(string name, out HoyolabApp gameType)
        {
            return GameTypeTable.TryGetValue(name, out gameType);
        }

        static HoyolabAppExtension()
        {
            GameTypeTable = Enum.GetValues<HoyolabApp>().Select(Current => KeyValuePair.Create(Current.GetDescription(), Current)).ToFrozenDictionary();
        }
    }
}