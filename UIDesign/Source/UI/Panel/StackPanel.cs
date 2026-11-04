using Common.Source.Extension;
using System.Windows;
using System.Windows.Controls;

namespace UIDesign.Source.UI.Panel
{
    public class StackPanel : System.Windows.Controls.StackPanel
    {
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(nameof(Spacing), typeof(double), typeof(StackPanel));

        protected override Size MeasureOverride(Size constraint)
        {
            Size panelSize = base.MeasureOverride(constraint);
            double accumulatedPosition = (InternalChildren.Count - 1) * Spacing;
            if (accumulatedPosition > 0)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    panelSize.Width += accumulatedPosition;
                }
                else
                {
                    panelSize.Height += accumulatedPosition;
                }
            }
            return panelSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            double accumulatedPosition = 0;
            UIElementCollection children = InternalChildren;
            for (int i = 0; i < children.Count; i++)
            {
                UIElement child = children[i];
                if (child.IsNull())
                {
                    continue;
                }
                Size childSize = child.DesiredSize;
                Rect finalRect;
                if (Orientation == Orientation.Horizontal)
                {
                    finalRect = new Rect(accumulatedPosition, 0, childSize.Width, arrangeSize.Height);
                }
                else
                {
                    finalRect = new Rect(0, accumulatedPosition, arrangeSize.Width, childSize.Height);
                }
                child.Arrange(finalRect);
                if (Orientation == Orientation.Horizontal)
                {
                    accumulatedPosition += childSize.Width + Spacing;
                }
                else
                {
                    accumulatedPosition += childSize.Height + Spacing;
                }
            }
            return arrangeSize;
        }
    }
}