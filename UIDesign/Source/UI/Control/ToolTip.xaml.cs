using System.Windows;

namespace UIDesign.Source.UI.Control
{
    public partial class ToolTip : System.Windows.Controls.ToolTip
    {
        public ToolTip()
        {
            InitializeComponent();
        }

        public ToolTip(string text) : this()
        {
            Text = text;
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ToolTip));

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(ToolTip));
    }
}