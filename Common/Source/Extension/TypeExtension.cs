using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Common.Source.Extension
{
    public static class TypeExtension
    {
        [StackTraceHidden]
        [DebuggerStepThrough]
        public static string GetDescription(this Type type, string name)
        {
            FieldInfo? field = type.GetField(name);
            ArgumentNullException.ThrowIfNull(field, nameof(field));
            return field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
        }
    }
}