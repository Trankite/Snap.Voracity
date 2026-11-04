using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Web.Hoyolab.Bbs.Forum.Newest
{
    public static class NewestRequestBuilderFactoryExtension
    {
        [DebuggerStepThrough]
        public static NewestRequestBuilderFactory SetZoneType(this NewestRequestBuilderFactory builder, ZoneType value)
        {
            return builder.Configure(builder.ZoneType = value);
        }

        [DebuggerStepThrough]
        public static NewestRequestBuilderFactory SetSortType(this NewestRequestBuilderFactory builder, SortType value)
        {
            return builder.Configure(builder.SortType = value);
        }

        [DebuggerStepThrough]
        public static NewestRequestBuilderFactory SetPageSize(this NewestRequestBuilderFactory builder, int value)
        {
            return builder.Configure(builder.PageSize = value);
        }
    }
}