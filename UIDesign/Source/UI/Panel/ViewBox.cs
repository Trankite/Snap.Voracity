using Common.Source.Extension;
using System.Windows;
using System.Windows.Controls;

namespace UIDesign.Source.UI.Panel
{
    public class ViewBox : Viewbox
    {
        public double MinScale
        {
            get => (double)GetValue(MinScaleProperty);
            set => SetValue(MinScaleProperty, value);
        }

        public static readonly DependencyProperty MinScaleProperty = DependencyProperty.Register(nameof(MinScale), typeof(double), typeof(ViewBox));

        public double MaxScale
        {
            get => (double)GetValue(MaxScaleProperty);
            set => SetValue(MaxScaleProperty, value);
        }

        public static readonly DependencyProperty MaxScaleProperty = DependencyProperty.Register(nameof(MaxScale), typeof(double), typeof(ViewBox), new PropertyMetadata(1D));

        protected override Size MeasureOverride(Size constraint)
        {
            if (Child.IsNull()) return constraint;
            Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size ChildSize = Child.DesiredSize;
            if (ChildSize.Width == 0 || ChildSize.Height == 0) return ChildSize;
            double ScaleWidth = constraint.Width / ChildSize.Width;
            double ScaleHeight = constraint.Height / ChildSize.Height;
            double FinalScale = Math.Min(ScaleWidth, ScaleHeight).Clamp(MinScale, MaxScale);
            double FinalWidth = Math.Min(ChildSize.Width * FinalScale, constraint.Width);
            double FinalHeight = Math.Min(ChildSize.Height * FinalScale, constraint.Height);
            Child.Measure(new Size(FinalWidth / FinalScale, FinalHeight / FinalScale));
            return new Size(FinalWidth, FinalHeight);
        }
    }
}