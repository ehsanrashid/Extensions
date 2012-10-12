using System.Windows.Documents;

namespace System.Windows.Controls
{
    public static class RichTextBoxExtensions
    {

        public static String GetText(this RichTextBox richTextBox)
        {
            var document = richTextBox.Document;
            return (default(FlowDocument) != document)
                ? new TextRange(document.ContentStart, document.ContentEnd).Text
                : String.Empty;
        }

        public static void SetText(this RichTextBox richTextBox, String text)
        {
            var document = richTextBox.Document;
            if (default(FlowDocument) == document) document = new FlowDocument();

            var blocks = document.Blocks;
            blocks.Clear();

            var paragraph = new Paragraph(new Run(text))
                            {
                                //Margin = new Thickness(0.3);
                            };

            blocks.Add(paragraph);
        }




    }
}
