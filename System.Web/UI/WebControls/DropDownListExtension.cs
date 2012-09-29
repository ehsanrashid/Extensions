namespace System.Web.UI.WebControls
{
    public static class DropDownListExtension
    {
        /// <summary>
        /// Selects the item in the list control that contains the specified value, if it exists.
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="valueSelect">The value of the item in the list control to select</param>
        /// <returns>Returns true if the value exists in the list control, false otherwise</returns>
        public static Boolean SelectValue(this DropDownList dropDownList, String valueSelect)
        {
            var listItem = dropDownList.Items.FindByValue(valueSelect);
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
        /// <param name="textSelect">The text of the item in the list control to select</param>
        /// <returns>Returns true if the text exists in the list control, false otherwise</returns>
        public static Boolean SelectText(this DropDownList dropDownList, String textSelect)
        {
            var listItem = dropDownList.Items.FindByText(textSelect);
            if (null != listItem)
            {
                listItem.Selected = true;
                return true;
            }
            return false;
        }
    }
}
