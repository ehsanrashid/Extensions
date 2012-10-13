namespace System.Windows
{
    using Linq;
    using Controls;

    public static class FrameworkElementExtension
    {

        /// <summary>
        /// Brings the control to front by setting maximum z index
        /// </summary>
        /// <param name="control">Current Element</param>
        public static void BringToFront(this FrameworkElement control)
        {
            if (null == control) return;

            var parent = control.Parent as Panel;
            if (null == parent) return;

            var maxZ = parent.Children.OfType<UIElement>()
                             .Where(x => !x.Equals(control))
                             .Select(Panel.GetZIndex)
                             .Max();

            Panel.SetZIndex(control, maxZ + 1);
        }

    }
}