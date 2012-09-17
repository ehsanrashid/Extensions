
namespace System.Web.UI
{
    public static class ControlCollectionExtension
    {
        /// <summary>
        /// Runs action delegate for all controls and subcontrols in ControlCollection.
        /// </summary>
        /// <param name="controlCollection">The control collection.</param>
        /// <param name="action">The action.</param>
        /// <remarks></remarks>
        public static void ForEachControl(this ControlCollection controlCollection, Action<Control> action)
        {
            if (default(ControlCollection) == controlCollection) return;
            foreach (Control control in controlCollection)
            {
                action(control);
                if (control.HasControls()) ForEachControl(control.Controls, action);
            }
        }
    }
}
