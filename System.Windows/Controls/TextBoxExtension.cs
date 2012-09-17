using System.Windows.Input;

namespace System.Windows.Controls
{
    public static class TextBoxExtension
    {

        public static void SetInputScope(this TextBox tb, InputScopeNameValue inputScopeNameValue)
        {
            tb.InputScope = new InputScope
            {
                Names = { new InputScopeName { NameValue = inputScopeNameValue } }
            };
        }

    }
}