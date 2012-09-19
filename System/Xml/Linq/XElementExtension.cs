namespace System.Xml.Linq
{
    public static class XElementExtension
    {
        public static XmlElement ToXmlElement(this XElement element)
        {
            var doc = new XmlDocument();
            doc.LoadXml(element.ToString());
            return doc.DocumentElement;
        }

    }
}
