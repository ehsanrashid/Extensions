namespace System.Xml
{
    using Linq;

    public static class XmlElementExtension
    {
        public static XElement ToXElement(this XmlElement element)
        {
            return XElement.Parse(element.OuterXml);
        }
    }
}
