using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Common.Source.Extension
{
    public static class TypeExtension
    {
        [DebuggerStepThrough]
        public static string GetDescription(this Type type, string name)
        {
            return type.GetField(name).Captured(field => field.IsNotNull() ? field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name : name);
        }
    }
}