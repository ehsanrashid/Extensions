using System.Linq;

namespace System.Web.UI.WebControls
{
    public static class ListControlExtension
    {

        public static ListItemCollection SelectedItems(this ListControl listControl)
        {
            var colSelectedListItems = new ListItemCollection();

            //foreach (var item in listControl.Items)
            //{
            //    if (item.Selected)
            //    {
            //        colSelectedListItems.Add(item);
            //    }
            //}
            
            foreach (var item in Enumerable.Cast<ListItem>(listControl.Items).Where((item) => item.Selected))
            {
                colSelectedListItems.Add(item);
            }
            return colSelectedListItems;
        }

    }
}
