namespace System.Web.UI.WebControls
{
    using Collections.Generic;
    using Linq;

    public static class ListControlExtension
    {

        //public static ListItemCollection SelectedItems(this ListControl listControl)
        //{
        //    var colSelectedListItems = new ListItemCollection();

        //    //foreach (var item in listControl.Items)
        //    //{
        //    //    if (item.Selected)
        //    //    {
        //    //        colSelectedListItems.Add(item);
        //    //    }
        //    //}

        //    foreach (var item in listControl.Items.Cast<ListItem>().Where((item) => item.Selected))
        //    {
        //        colSelectedListItems.Add(item);
        //    }
        //    return colSelectedListItems;
        //}

        public static IEnumerable<ListItem> SelectedItems(this ListControl listControl)
        {
            return listControl.Items.Cast<ListItem>().Where((item) => item.Selected);//.Select((item) => item);
        }

        //public static StringCollection SelectedValues(this ListControl listControl)
        //{
        //    var colSelectedValues = new StringCollection();

        //    foreach (var item in listControl.Items.Cast<ListItem>().Where((item) => item.Selected).Select((item) => item.Value))
        //    {
        //        colSelectedValues.Add(item);
        //    }
        //    return colSelectedValues;
        //}

        public static IEnumerable<String> SelectedValues(this ListControl listControl)
        {
            return listControl.Items.Cast<ListItem>().Where((item) => item.Selected).Select((item) => item.Value);
        }

        public static IEnumerable<String> SelectedTexts(this ListControl listControl)
        {
            return listControl.Items.Cast<ListItem>().Where((item) => item.Selected).Select((item) => item.Text);
        }

    }
}
