namespace System.Windows
{
    using Controls;
    using Interop;

    public static class WindowExtension
    {
        /// <summary>
        /// Set the initial focus on the given control
        /// </summary>
        /// <param name="window">Childwindow for which the focus must be set</param>
        /// <param name="controlFocus">Control to set the focus on</param>
        public static void SetInitialFocus(this Window window, Control controlFocus)
        {
            RoutedEventHandler routedHandler = null; // set to null to prevent unassigned compiler error
            routedHandler = delegate(object sender, RoutedEventArgs e)
            {
                controlFocus.Focus();
                var txtBox = controlFocus as TextBox;
                if (txtBox != null)
                {
                    txtBox.SelectAll();
                }
                // unsubscribe after first execute
                window.GotFocus -= routedHandler;
            };

            window.GotFocus += routedHandler;
        }


        public static bool? ShowDialog(this Window win, IntPtr handle)
        {
            var helper = new WindowInteropHelper(win) { Owner = handle };
            return win.ShowDialog();
        }
    }
}