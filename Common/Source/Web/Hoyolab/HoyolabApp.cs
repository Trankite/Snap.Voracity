using System.ComponentModel;

namespace Common.Source.Web.Hoyolab
{
    [Flags]
    public enum HoyolabApp
    {
        [Description("bbs_cn")]
        HoyolabChina = 0x01,

        [Description("bbs_global")]
        HoyolabGlobal = 0x02,

        [Description("hk4e_cn")]
        GenshinChina = 0x04,

        [Description("hk4e_global")]
        GenshinGlobal = 0x08,

        [Description("hkrpg_cn")]
        StarRailChina = 0x10,

        [Description("hkrpg_global")]
        StarRailGlobal = 0x20,

        [Description("bh3_cn")]
        Honkai3China = 0x40,

        [Description("bh3_global")]
        Honkai3Global = 0x80,

        [Description("nap_cn")]
        ZenlessChina = 0x100,

        [Description("nap_global")]
        ZenlessGlobal = 0x200,
    }
}