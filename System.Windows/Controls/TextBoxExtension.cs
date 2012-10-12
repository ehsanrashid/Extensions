namespace System.Windows.Controls
{
    using Input;
    using Threading;
    

    public static class TextBoxExtension
    {

        public static void SetInputScope(this TextBox tb, InputScopeNameValue inputScopeNameValue)
        {
            tb.InputScope = new InputScope
            {
                Names = { new InputScopeName { NameValue = inputScopeNameValue } }
            };
        }


        ///
        /// usage:  control.SetText(text);
        ///
        // Delegates to enable async calls for setting controls properties
        private delegate void SetTextCallback(TextBox textBox, String text);

        // Thread safe updating of control's text property
        public static void SetText(this TextBox textBox, String text)
        {
            if (textBox.Dispatcher.CheckAccess())
            {
                textBox.Text = text;
            }
            else
            {
                SetTextCallback method = new SetTextCallback(SetText);
                textBox.Dispatcher.Invoke(method, new Object[] { textBox, text });
            }
        }
    }
}