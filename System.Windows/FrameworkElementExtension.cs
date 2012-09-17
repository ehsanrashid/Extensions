using System.Linq;
using System.Windows.Controls;

namespace System.Windows
{
    public static class FrameworkElementExtension
    {

        /// <summary>
        /// Brings the control to front by setting maximum z index
        /// </summary>
        /// <param name="control">Current Element</param>
        public static void BringToFront(this FrameworkElement control)
        {
            if (control == null) return;

            var parent = control.Parent as Panel;
            if (parent == null) return;

            int maxZ = parent.Children.OfType<UIElement>()
                .Where(x => x != control)
                .Select(Panel.GetZIndex)
                .Max();
            Panel.SetZIndex(control, maxZ + 1);
        }

    }
}