using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIDesign.Source.UI.Control
{
    public partial class Slider : System.Windows.Controls.Slider
    {
        private static readonly Duration ThumbDuration = new(TimeSpan.FromMilliseconds(500));

        public Slider()
        {
            InitializeComponent();
            Model = new SliderModel();
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            double Target = GetTickValue(Value);
            DoubleAnimation Animation = new(Target, ThumbDuration);
            Animation.Completed += (sender, e) =>
            {
                Value = Target;
                BeginAnimation(ValueProperty, null);
            };
            BeginAnimation(ValueProperty, Animation);
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            if (Math.Abs(Model.Value - newValue) >= SmallChange)
            {
                Model.Value = GetTickValue(newValue);
            }
        }

        private double GetTickValue(double value)
        {
            return Math.Round(value / TickFrequency) * TickFrequency;
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            Model.Minimun = newMinimum;
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            Model.Maximum = newMaximum;
        }

        public SliderModel Model
        {
            get => (SliderModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(SliderModel), typeof(Slider));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(Slider), new PropertyMetadata(default(string), TitleChangedCallback));

        private static void TitleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider ScopedSlider)
            {
                ScopedSlider.Model.Title = ScopedSlider.Title;
            }
        }

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(Slider));
    }
}