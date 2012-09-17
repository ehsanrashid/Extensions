using System.Linq;

namespace System.Xml
{
    /// <summary>
    ///   Extension methods for the XmlNode / XmlDocument classes and its sub classes
    /// </summary>
    public static class XmlNodeExtension
    {
        /// <summary>
        ///   Appends a child to a XML node
        /// </summary>
        /// <param name="xmlNode"> The parent node. </param>
        /// <param name="name"> The name of the child node. </param>
        /// <param name="namespaceUri"> The node namespace. </param>
        /// <returns> The newly cerated XML node </returns>
        public static XmlNode CreateChildNode(this XmlNode xmlNode, String name, String namespaceUri)
        {
            var document = (xmlNode is XmlDocument) ? (XmlDocument) xmlNode : xmlNode.OwnerDocument;
            if (default(XmlDocument) == document) return default(XmlNode);
            var node = document.CreateElement(name, namespaceUri);
            xmlNode.AppendChild(node);
            return node;
        }

        /// <summary>
        ///   Appends a child to a XML node
        /// </summary>
        /// <param name="xmlNode"> The parent node. </param>
        /// <param name="name"> The name of the child node. </param>
        /// <returns> The newly cerated XML node </returns>
        public static XmlNode CreateChildNode(this XmlNode xmlNode, String name)
        {
            var document = (xmlNode is XmlDocument) ? (XmlDocument) xmlNode : xmlNode.OwnerDocument;
            if (default(XmlDocument) == document) return default(XmlNode);
            var node = document.CreateElement(name);
            xmlNode.AppendChild(node);
            return node;
        }

        /// <summary>
        ///   Appends a CData section to a XML node and prefills the provided data
        /// </summary>
        /// <param name="xmlNode"> The parent node. </param>
        /// <param name="data"> The CData section value. </param>
        /// <returns> The created CData Section </returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode xmlNode, String data)
        {
            var document = (xmlNode is XmlDocument) ? (XmlDocument) xmlNode : xmlNode.OwnerDocument;
            if (default(XmlDocument) == document) return default(XmlCDataSection);
            var node = document.CreateCDataSection(data);
            xmlNode.AppendChild(node);
            return node;
        }

        /// <summary>
        ///   Appends a CData section to a XML node
        /// </summary>
        /// <param name="xmlNode"> The parent node. </param>
        /// <returns> The created CData Section </returns>
        public static XmlCDataSection CreateCDataSection(this XmlNode xmlNode)
        {
            return xmlNode.CreateCDataSection(String.Empty);
        }

        /// <summary>
        ///   Returns the value of a nested CData section.
        /// </summary>
        /// <param name="xmlNode"> The parent node. </param>
        /// <returns> The CData section content </returns>
        public static String GetCDataSection(this XmlNode xmlNode)
        {
            //foreach (var node in parentNode.ChildNodes)
            //{
            //    if (node is XmlCDataSection)
            //        return ((XmlCDataSection) node).Value;
            //}
            //return null;
            return xmlNode.ChildNodes.OfType<XmlCDataSection>().Select(node => (node).Value).FirstOrDefault();
        }

        /// <summary>
        ///   Gets an attribute value
        /// </summary>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="attributeName"> The Name of the attribute. </param>
        /// <param name="defaultValue"> The default value to be returned if no matching attribute exists. </param>
        /// <returns> The attribute value </returns>
        public static String GetAttribute(this XmlNode xmlNode, String attributeName, String defaultValue)
        {
            if (default(XmlNode) == xmlNode || default(XmlAttributeCollection) == xmlNode.Attributes)
                return default(String);
            var attribute = xmlNode.Attributes[attributeName];
            return (default(XmlAttribute) != attribute) ? attribute.InnerText : defaultValue;
        }

        /// <summary>
        ///   Gets an attribute value
        /// </summary>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="attributeName"> The Name of the attribute. </param>
        /// <returns> The attribute value </returns>
        public static String GetAttribute(this XmlNode xmlNode, String attributeName)
        {
            return GetAttribute(xmlNode, attributeName, default(String));
        }

        /// <summary>
        ///   Gets an attribute value converted to the specified data type
        /// </summary>
        /// <typeparam name="T"> The desired return data type </typeparam>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="attributeName"> The Name of the attribute. </param>
        /// <param name="defaultValue"> The default value to be returned if no matching attribute exists. </param>
        /// <returns> The attribute value </returns>
        public static T GetAttribute<T>(this XmlNode xmlNode, String attributeName, T defaultValue)
        {
            var value = GetAttribute(xmlNode, attributeName);
            return value.IsNotNullOrEmpty()
                       ? (typeof (Type) == typeof (T)
                              ? (T) (Object) Type.GetType(value, true)
                              : value.ConvertTo(defaultValue))
                       : defaultValue;
        }

        /// <summary>
        ///   Gets an attribute value converted to the specified data type
        /// </summary>
        /// <typeparam name="T"> The desired return data type </typeparam>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="attributeName"> The Name of the attribute. </param>
        /// <returns> The attribute value </returns>
        public static T GetAttribute<T>(this XmlNode xmlNode, String attributeName)
        {
            return GetAttribute(xmlNode, attributeName, default(T));
        }

        /// <summary>
        ///   Creates or updates an attribute with the passed value.
        /// </summary>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        public static void SetAttribute(this XmlNode xmlNode, String name, String value)
        {
            if (default(XmlNode) == xmlNode || default(XmlAttributeCollection) == xmlNode.Attributes) return;
            var attribute = xmlNode.Attributes[name, xmlNode.NamespaceURI];
            if (default(XmlAttribute) == attribute)
            {
                if (default(XmlDocument) != xmlNode.OwnerDocument)
                    attribute = xmlNode.OwnerDocument.CreateAttribute(name, xmlNode.OwnerDocument.NamespaceURI);
                xmlNode.Attributes.Append(attribute);
            }
            attribute.InnerText = value;
        }

        /// <summary>
        ///   Creates or updates an attribute with the passed value.
        /// </summary>
        /// <param name="xmlNode"> The node. </param>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        public static void SetAttribute(this XmlNode xmlNode, String name, Object value)
        {
            SetAttribute(xmlNode, name, (default(Object) != value) ? value.ToString() : default(String));
        }

    }
}