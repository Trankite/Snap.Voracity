using StarRailDamage.Source.UI.Model.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class ScoreboardSpan : UserControl
    {
        public ScoreboardSpan()
        {
            InitializeComponent();
            Model = new ScoreboardSpanModel();
        }

        public ScoreboardSpanModel Model
        {
            get => (ScoreboardSpanModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(ScoreboardSpanModel), typeof(ScoreboardSpan));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ScoreboardSpan), new PropertyMetadata(default(string), TitleChangedCallback));

        private static void TitleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScoreboardSpan ScoreboardSpan)
            {
                ScoreboardSpan.Model.Title = ScoreboardSpan.Title;
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(ScoreboardSpan), new PropertyMetadata(default(double), ValueChangedCallback));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScoreboardSpan ScoreboardSpan)
            {
                ScoreboardSpan.Percent = (ScoreboardSpan.Model.Value = ScoreboardSpan.Value) / ScoreboardSpan.TempValue - 1;
            }
        }

        public double TempValue
        {
            get => (double)GetValue(TempValueProperty);
            set => SetValue(TempValueProperty, value);
        }

        public static readonly DependencyProperty TempValueProperty = DependencyProperty.Register(nameof(TempValue), typeof(double), typeof(ScoreboardSpan), new PropertyMetadata(default(double), TempValueChangedCallback));

        private static void TempValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScoreboardSpan ScoreboardSpan)
            {
                ScoreboardSpan.Model.TempValue = ScoreboardSpan.TempValue;
            }
        }

        public double Percent
        {
            get => (double)GetValue(PercentProperty);
            set => SetValue(PercentProperty, value);
        }

        public static readonly DependencyProperty PercentProperty = DependencyProperty.Register(nameof(Percent), typeof(double), typeof(ScoreboardSpan), new PropertyMetadata(default(double), PercentChangedCallback));

        private static void PercentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScoreboardSpan ScoreboardSpan)
            {
                ScoreboardSpan.TempBrush = (ScoreboardSpan.Model.Percent = ScoreboardSpan.Percent) >= 0 ? ScoreboardSpan.PlusBrush : ScoreboardSpan.MinuBrush;
            }
        }

        public SolidColorBrush PlusBrush
        {
            get => (SolidColorBrush)GetValue(PlusBrushProperty);
            set => SetValue(PlusBrushProperty, value);
        }

        public static readonly DependencyProperty PlusBrushProperty = DependencyProperty.Register(nameof(PlusBrush), typeof(SolidColorBrush), typeof(ScoreboardSpan));

        public SolidColorBrush MinuBrush
        {
            get => (SolidColorBrush)GetValue(MinuBrushProperty);
            set => SetValue(MinuBrushProperty, value);
        }

        public static readonly DependencyProperty MinuBrushProperty = DependencyProperty.Register(nameof(MinuBrush), typeof(SolidColorBrush), typeof(ScoreboardSpan));

        public SolidColorBrush TempBrush
        {
            get => (SolidColorBrush)GetValue(TempBrushProperty);
            set => SetValue(TempBrushProperty, value);
        }

        public static readonly DependencyProperty TempBrushProperty = DependencyProperty.Register(nameof(TempBrush), typeof(SolidColorBrush), typeof(ScoreboardSpan));

        public double DropFontSize
        {
            get => (double)GetValue(DropFontSizeProperty);
            set => SetValue(DropFontSizeProperty, value);
        }

        public static readonly DependencyProperty DropFontSizeProperty = DependencyProperty.Register(nameof(DropFontSize), typeof(double), typeof(ScoreboardSpan));
    }
}