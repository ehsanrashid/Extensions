namespace System.Web.UI.WebControls
{
    public static class DropDownListExtension
    {
        /// <summary>
        /// Selects the item in the list control that contains the specified value, if it exists.
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="selectedValue">The value of the item in the list control to select</param>
        /// <returns>Returns true if the value exists in the list control, false otherwise</returns>
        public static Boolean SetSelectedValue(this DropDownList dropDownList, String selectedValue)
        {
            var listItem = dropDownList.Items.FindByValue(selectedValue);
            if (null != listItem)
            {
                listItem.Selected = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Selects the item in the list control that contains the specified text, if it exists.
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="selectedText">The text of the item in the list control to select</param>
        /// <returns>Returns true if the text exists in the list control, false otherwise</returns>
        public static Boolean SetSelectedText(this DropDownList dropDownList, String selectedText)
        {
            var listItem = dropDownList.Items.FindByText(selectedText);
            if (null != listItem)
            {
                listItem.Selected = true;
                return true;
            }
            return false;
        }

    }
}
