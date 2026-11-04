using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UIDesign.Source.Core.Setting;

namespace UIDesign.Source.UI.Control
{
    public partial class TextBlock : System.Windows.Controls.TextBlock
    {
        public TextBlock()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }

        public string TipText
        {
            get => (string)GetValue(TipTextProperty);
            set => SetValue(TipTextProperty, value);
        }

        private static readonly DependencyProperty TipTextProperty = DependencyProperty.Register(nameof(TipText), typeof(string), typeof(TextBlock), new PropertyMetadata(default, TipTextChangedCallBack));

        private static void TipTextChangedCallBack(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependency is TextBlock textBlock)
            {
                SetToolTip(textBlock, eventArguments.NewValue as string);
            }
        }

        public bool TipOnlyTrim
        {
            get => (bool)GetValue(TipOnlyTrimProperty);
            set => SetValue(TipOnlyTrimProperty, value);
        }

        private static readonly DependencyProperty TipOnlyTrimProperty = DependencyProperty.Register(nameof(TipOnlyTrim), typeof(bool), typeof(TextBlock), new PropertyMetadata(false, TipOnlyTrimChangedCallBack));

        private static void TipOnlyTrimChangedCallBack(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArguments)
        {
            if (dependency is TextBlock textBlock)
            {
                textBlock.EnabledToolTip();
            }
        }

        private static void SetToolTip(TextBlock textBlock, string? tipText)
        {
            if (string.IsNullOrEmpty(tipText))
            {
                textBlock.ToolTip = default;
            }
            else
            {
                if (textBlock.ToolTip is ToolTip toolTip)
                {
                    toolTip.Text = tipText;
                }
                else
                {
                    textBlock.ToolTip = new ToolTip(tipText);
                    textBlock.EnabledToolTip();
                }
            }
        }

        public Size GetTextSize()
        {
            Typeface typeface = new(FontFamily, FontStyle, FontWeight, FontStretch);
            FormattedText formattedText = new(Text, CultureInfo.CurrentCulture, FlowDirection, typeface, FontSize, Foreground, AppSetting.PixelsPerDip);
            return new Size(formattedText.Width, formattedText.Height);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs eventArguments)
        {
            EnabledToolTip();
        }

        private void EnabledToolTip()
        {
            ToolTipService.SetIsEnabled(this, !TipOnlyTrim || IsTextTrimmed());
        }

        public bool IsTextTrimmed()
        {
            return GetTextSize().Width - 0.1 > ActualWidth;
        }
    }
}