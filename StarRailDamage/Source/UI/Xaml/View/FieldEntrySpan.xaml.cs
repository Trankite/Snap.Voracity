using StarRailDamage.Source.UI.Model.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class FieldEntrySpan : UserControl
    {
        public FieldEntrySpan()
        {
            InitializeComponent();
            Model = new FieldEntrySpanModel();
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox TextBox)
            {
                if (TextBox.Text == "0") TextBox.Text = string.Empty;
            }
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Focus();
        }

        public FieldEntrySpanModel Model
        {
            get => (FieldEntrySpanModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(FieldEntrySpanModel), typeof(FieldEntrySpan));

        public BitmapImage Icon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(BitmapImage), typeof(FieldEntrySpan), new PropertyMetadata(default(BitmapImage), IconChangedCallback));

        private static void IconChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldEntrySpan FieldEntrySpan)
            {
                FieldEntrySpan.Model.Icon = FieldEntrySpan.Icon;
            }
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(FieldEntrySpan), new PropertyMetadata(default(string), TitleChangedCallback));

        private static void TitleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldEntrySpan FieldEntrySpan)
            {
                FieldEntrySpan.Model.Title = FieldEntrySpan.Title;
            }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(FieldEntrySpan), new PropertyMetadata(default(string), TextChangedCallback, TextCoerceValueCallback));

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldEntrySpan FieldEntrySpan)
            {
                FieldEntrySpan.Model.Text = FieldEntrySpan.Text;
            }
        }

        private static string TextCoerceValueCallback(DependencyObject d, object baseValue)
        {
            return (double.TryParse((string)baseValue, out double result) ? Math.Round(result, ((FieldEntrySpan)d).Digits) : 0).ToString();
        }

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(FieldEntrySpan), new PropertyMetadata(default(string), UnitChangedCallback));

        private static void UnitChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldEntrySpan FieldEntrySpan)
            {
                FieldEntrySpan.Model.Unit = FieldEntrySpan.Unit;
            }
        }

        public int Digits
        {
            get => (int)GetValue(DigitsProperty);
            set => SetValue(DigitsProperty, value);
        }

        public static readonly DependencyProperty DigitsProperty = DependencyProperty.Register(nameof(Digits), typeof(int), typeof(FieldEntrySpan), new PropertyMetadata(default(int), DigitsChangedCallback));

        private static void DigitsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FieldEntrySpan FieldEntrySpan)
            {
                FieldEntrySpan.Model.Digits = FieldEntrySpan.Digits;
            }
        }

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(FieldEntrySpan));

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(FieldEntrySpan));
    }
}