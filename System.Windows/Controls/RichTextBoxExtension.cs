using System.Windows.Documents;

namespace System.Windows.Controls
{
    public static class RichTextBoxExtensions
    {
        public static String GetText(this RichTextBox richTextBox)
        {
            if (richTextBox.Document != default(FlowDocument))
            {
                var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                return textRange.Text;
            }
            return String.Empty;
        }

        public static void SetText(this RichTextBox richTextBox, String text)
        {
            if (richTextBox.Document == default(FlowDocument))
            {
                richTextBox.Document = new FlowDocument();
            }

            var paragraph = new Paragraph(new Run(text));
            //paragraph.Margin = new Thickness(0.3);

            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(paragraph);
        }


     

    }
}
