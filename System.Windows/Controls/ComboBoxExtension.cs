
namespace System.Windows.Controls
{
    public static class ComboBoxExtension
    {

        public static ComboBoxItem GetSelectedComboItem(this ComboBox comboBox)
        {
            var selectedItem = comboBox.SelectedItem;
            ComboBoxItem cbi = comboBox.ItemContainerGenerator.ContainerFromItem(selectedItem) as ComboBoxItem;
            return cbi;
        }


    }
}
