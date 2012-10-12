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
            var fieldInfo = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                var key = fieldInfo.GetValue(button);
                var propInfo = button.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                var list = (EventHandlerList) propInfo.GetValue(button, null);
                list.RemoveHandler(key, list[key]);
            }
        }

    }
}