using StarRailDamage.Source.Extension;
using StarRailDamage.Source.UI.Model.View;
using StarRailDamage.Source.UI.Xaml.Control;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class FormulaSpan : UserControl
    {
        public FormulaSpan()
        {
            InitializeComponent();
            Model = new FormulaSpanModel();
        }

        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                Dropdown = !Dropdown.Configure(e.Handled = true);
            }
            else if (e.Key == Key.Enter)
            {
                Focus().Configure(Dropdown = false);
            }
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void PromptItemClick(object sender, RoutedEventArgs e)
        {
            ScopedTextBox TextBox = GetTextBox();
            int CaretIndex = TextBox.CaretIndex;
            string Prompt = $"[{((ScopedButton)sender).Content}]";
            TextBox.Text = TextBox.Text.Insert(CaretIndex, Prompt);
            TextBox.CaretIndex = CaretIndex + Prompt.Length;
            TextBox.Focus().Configure(Dropdown = false);
        }

        private ScopedTextBox GetTextBox() => (ScopedTextBox)Template.FindName("PART_TextBox", this);

        public FormulaSpanModel Model
        {
            get => (FormulaSpanModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(FormulaSpanModel), typeof(FormulaSpan));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(FormulaSpan), new PropertyMetadata(default(string), TextChangedCallback));

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormulaSpan FormulaSpan)
            {
                FormulaSpan.Model.Text = FormulaSpan.Text;
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(FormulaSpan), new PropertyMetadata(default(double), ValueChangedCallback));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormulaSpan FormulaSpan)
            {
                FormulaSpan.Model.Value = FormulaSpan.Value;
            }
        }

        public ObservableCollection<string> Items
        {
            get => (ObservableCollection<string>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<string>), typeof(FormulaSpan));

        public bool Dropdown
        {
            get => (bool)GetValue(DropdownProperty);
            set => SetValue(DropdownProperty, value);
        }

        public static readonly DependencyProperty DropdownProperty = DependencyProperty.Register(nameof(Dropdown), typeof(bool), typeof(FormulaSpan));

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(FormulaSpan));
    }
}