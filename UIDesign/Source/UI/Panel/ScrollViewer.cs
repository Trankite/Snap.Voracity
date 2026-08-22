using System.Windows.Controls;
using System.Windows.Input;

namespace UIDesign.Source.UI.Panel
{
    public class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled)
            {
                if (HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled) return;
                ScrollToHorizontalOffset(HorizontalOffset - e.Delta); e.Handled = true;
            }
            else if (e.Delta >= 0 ? VerticalOffset > 0 : ViewportHeight + VerticalOffset < ExtentHeight)
            {
                ScrollToVerticalOffset(VerticalOffset - e.Delta);
            }
        }
    }
}