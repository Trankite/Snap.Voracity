using System.Windows.Controls;
using System.Windows.Input;

namespace UIDesign.Source.UI.Panel
{
    public class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs eventArguments)
        {
            if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled)
            {
                if (HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled)
                {
                    return;
                }
                ScrollToHorizontalOffset(HorizontalOffset - eventArguments.Delta);
                eventArguments.Handled = true;
            }
            else if (eventArguments.Delta >= 0 ? VerticalOffset > 0 : ViewportHeight + VerticalOffset < ExtentHeight)
            {
                ScrollToVerticalOffset(VerticalOffset - eventArguments.Delta);
            }
        }
    }
}