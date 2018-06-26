namespace KbUtil.Lib.Deserialization.Internal
{
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    internal static class XmlUtilities
    {
        public static bool TryGetAttribute(XElement xElement, string attributeName, out XAttribute attribute)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(attributeName))
            {
                attribute = null;
                return false;
            }

            attribute = xElement
                .Attributes()
                .FirstOrDefault(attr => attr.Name.ToString() == attributeName);

            return !(attribute is default(XAttribute));
        }

        public static bool TryGetSubElement(XElement xElement, string elementName, out XElement element)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(elementName))
            {
                element = null;
                return false;
            }

            element = (XElement)xElement
                .Nodes()
                .FirstOrDefault(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement)node).Name.ToString() == elementName);

            return !(element is default(XElement));
        }
    }
}
