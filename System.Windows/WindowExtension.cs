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
            var routedHandler = default(RoutedEventHandler); // set to null to prevent unassigned compiler error
            routedHandler = delegate
                            {
                                controlFocus.Focus();
                                var textBox = controlFocus as TextBox;
                                if (null != textBox)
                                {
                                    textBox.SelectAll();
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