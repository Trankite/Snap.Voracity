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

        private static void TipTextChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock ScopedTextBlock)
            {
                SetToolTip(ScopedTextBlock, e.NewValue as string);
            }
        }

        public bool TipOnlyTrim
        {
            get => (bool)GetValue(TipOnlyTrimProperty);
            set => SetValue(TipOnlyTrimProperty, value);
        }

        private static readonly DependencyProperty TipOnlyTrimProperty = DependencyProperty.Register(nameof(TipOnlyTrim), typeof(bool), typeof(TextBlock), new PropertyMetadata(false, TipOnlyTrimChangedCallBack));

        private static void TipOnlyTrimChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock ScopedTextBlock)
            {
                ScopedTextBlock.EnabledToolTip();
            }
        }

        private static void SetToolTip(TextBlock scopedTextBlock, string? tipText)
        {
            if (string.IsNullOrEmpty(tipText))
            {
                scopedTextBlock.ToolTip = default;
            }
            else
            {
                if (scopedTextBlock.ToolTip is ToolTip ScopedToolTip)
                {
                    ScopedToolTip.Text = tipText;
                }
                else
                {
                    scopedTextBlock.ToolTip = new ToolTip(tipText);
                    scopedTextBlock.EnabledToolTip();
                }
            }
        }

        public Size GetTextSize()
        {
            Typeface Typeface = new(FontFamily, FontStyle, FontWeight, FontStretch);
            FormattedText FormattedText = new(Text, CultureInfo.CurrentCulture, FlowDirection, Typeface, FontSize, Foreground, AppSetting.PixelsPerDip);
            return new Size(FormattedText.Width, FormattedText.Height);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => EnabledToolTip();

        private void EnabledToolTip() => ToolTipService.SetIsEnabled(this, !TipOnlyTrim || IsTextTrimmed());

        public bool IsTextTrimmed() => GetTextSize().Width - 0.1 > ActualWidth;
    }
}