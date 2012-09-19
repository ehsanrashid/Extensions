
namespace System.Xml
{
    using Linq;

    public static class XmlDocumentExtension
    {

        public static XDocument ToXDocument(this XmlDocument xmlDocument, LoadOptions options = LoadOptions.None)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader, options);
            }
        }

        //public static XDocument ToXDocument(this XmlDocument xmlDocument)
        //{
        //    return ToXDocument(xmlDocument, LoadOptions.None);
        //}
  
    }
}
