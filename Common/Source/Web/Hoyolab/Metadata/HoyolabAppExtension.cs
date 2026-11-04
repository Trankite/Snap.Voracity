using Common.Source.Extension;
using System.Collections.Frozen;

namespace Common.Source.Web.Hoyolab.Metadata
{
    public static class HoyolabAppExtension
    {
        private static readonly FrozenDictionary<string, HoyolabApp> GameTypeTable;

        public static bool TryGetValue(string name, out HoyolabApp hoyolabApp)
        {
            return GameTypeTable.TryGetValue(name, out hoyolabApp);
        }

        static HoyolabAppExtension()
        {
            GameTypeTable = Enum.GetValues<HoyolabApp>().Select(current => KeyValuePair.Create(current.GetDescription(), current)).ToFrozenDictionary();
        }
    }
}