using System.Text;
using System.Windows.Markup;
using System.Xml;

namespace System.Windows.Controls
{
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

            var strbuild = new StringBuilder();
            XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);
            XamlWriter.Save(control.Template, xmlwrite);
            return strbuild.ToString();
        }
    }
}