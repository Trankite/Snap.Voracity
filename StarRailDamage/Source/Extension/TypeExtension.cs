using System.ComponentModel;
using System.Reflection;

namespace StarRailDamage.Source.Extension
{
    public static class TypeExtension
    {
        public static string GetDescription(this Type type, string name)
        {
            return type.GetField(name).Captured(Field => Field.IsNotNull() ? Field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name : name);
        }
    }
}