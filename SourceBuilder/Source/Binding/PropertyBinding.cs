namespace SourceBuilder.Source.Binding
{
    public class PropertyBinding
    {
        public static void BuildCode(CodeBuilder builder, string name, string propertyType, string ownerType, string? defaultValue = null, bool callback = false)
        {
            builder.WriteLine($"public {propertyType} {name}");
            builder.WriteLine("{");
            builder.WriteLine($"get => ({propertyType})GetValue({name}Property);");
            builder.WriteLine($"set => SetValue({name}Property, value);");
            builder.WriteLine("}");
            builder.WriteLine();
            builder.WriteLine($"public static readonly DependencyProperty {name}Property = DependencyProperty.Register(nameof({name}), typeof({propertyType}), typeof({ownerType}){(string.IsNullOrEmpty(defaultValue) && !callback ? string.Empty : $", new PropertyMetadata({(string.IsNullOrEmpty(defaultValue) ? $"default({propertyType})" : defaultValue)}{(callback ? $", {name}ChangedCallback" : string.Empty)})")});");
            if (callback)
            {
                builder.WriteLine();
                builder.WriteLine($"private static void {name}ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)");
                builder.WriteLine("{");
                builder.WriteLine($"if (d is {ownerType} {ownerType})");
                builder.WriteLine("{");
                builder.WriteLine($"{ownerType}.Model.{name} = {ownerType}.{name};");
                builder.WriteLine("}");
                builder.WriteLine("}");
            }
            builder.WriteLine();
        }
    }
}