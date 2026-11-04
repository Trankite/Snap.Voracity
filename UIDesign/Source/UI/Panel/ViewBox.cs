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
            if (Child.IsNull())
            {
                return constraint;
            }
            Child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size childSize = Child.DesiredSize;
            if (childSize.Width == 0 || childSize.Height == 0)
            {
                return childSize;
            }
            double scaleWidth = constraint.Width / childSize.Width;
            double scaleHeight = constraint.Height / childSize.Height;
            double scale = Math.Min(scaleWidth, scaleHeight).Clamp(MinScale, MaxScale);
            double width = Math.Min(childSize.Width * scale, constraint.Width);
            double height = Math.Min(childSize.Height * scale, constraint.Height);
            Child.Measure(new Size(width / scale, height / scale));
            return new Size(width, height);
        }
    }
}