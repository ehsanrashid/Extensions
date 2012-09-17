using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Controls
{

    public static class ButtonExtension
    {
        /// <summary>
        /// Removes click event from button
        /// </summary>
        /// <param name="button"></param>
        public static void RemoveClickEvent(this Button button)
        {
            var f1 = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
            if (f1 != null)
            {
                var obj = f1.GetValue(button);
                var pi = button.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList) pi.GetValue(button, null);
                list.RemoveHandler(obj, list[obj]);
            }
        }
    }
}