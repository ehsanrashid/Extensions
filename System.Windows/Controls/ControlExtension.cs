namespace System.Windows.Controls
{
    using Xml;
    using Markup;
    using Text;

    public static class ControlExtension
    {
        /// <summary>
        /// Dump Control Templates of WPF Controls
        /// </summary>
        /// <param name="control">Control whose xaml content of ControlTemplate has to fethced</param>
        /// <returns>XAML representation of Control Template of the control</returns>
        public static string DumpControlTemplate(this Control control)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };

            var sbOutput = new StringBuilder();
            var xmlwrite = XmlWriter.Create(sbOutput, settings);
            XamlWriter.Save(control.Template, xmlwrite);
            return sbOutput.ToString();
        }


    }
}