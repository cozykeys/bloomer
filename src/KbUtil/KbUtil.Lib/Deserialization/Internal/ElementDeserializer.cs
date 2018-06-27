namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class ElementDeserializer : IDeserializer<Element>
    {
        public static ElementDeserializer Default { get; set; } = new ElementDeserializer();

        public void Deserialize(XElement xElement, Element element)
        {
            // Constants should be the first thing we deserialize for every element so that
            // every subsequent attribute/element that is deserialized can use them
            DeserializeConstants(xElement, element);

            DeserializeName(xElement, element);
            DeserializeXOffset(xElement, element);
            DeserializeYOffset(xElement, element);
            DeserializeRotation(xElement, element);
            DeserializeHeight(xElement, element);
            DeserializeWidth(xElement, element);
            DeserializeMargin(xElement, element);
            DeserializeDebug(xElement, element);
        }

        private static void DeserializeConstants(XElement xElement, Element element)
        {
            if (XmlUtilities.TryGetSubElement(xElement, "Constants", out XElement constantsElement))
            {
                IEnumerable<XElement> constantElements = constantsElement
                    .Nodes()
                    .Where(node =>
                        node.NodeType == System.Xml.XmlNodeType.Element)
                    .Select(node => (XElement)node);

                Dictionary<string, Constant> constants = constantElements
                    .Select(constantElement => DeserializeConstant(element, constantElement))
                    .ToDictionary(constant => constant.Name);

                element.Constants = constants;
            }
        }

        private static Constant DeserializeConstant(Element parent, XElement constantElement)
        {
            var constant = new Constant { Parent = parent };
            ConstantDeserializer.Default.Deserialize(constantElement, constant);
            return constant;
        }

        private void DeserializeName(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Name", out XAttribute nameAttribute))
            {
                element.Name = nameAttribute.ValueAsString(element);
            }
        }

        private void DeserializeXOffset(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "XOffset", out XAttribute xOffsetAttribute))
            {
                element.XOffset = xOffsetAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeYOffset(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "YOffset", out XAttribute yOffsetAttribute))
            {
                element.YOffset = yOffsetAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeRotation(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Rotation", out XAttribute rotationAttribute))
            {
                element.Rotation = rotationAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeHeight(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Height", out XAttribute heightAttribute))
            {
                element.Height = heightAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeWidth(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Width", out XAttribute widthAttribute))
            {
                element.Width = widthAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeMargin(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Margin", out XAttribute marginAttribute))
            {
                element.Margin = marginAttribute.ValueAsFloat(element);
            }
        }

        private void DeserializeDebug(XElement xElement, Element element)
        {
            if(XmlUtilities.TryGetAttribute(xElement, "Debug", out XAttribute debugAttribute))
            {
                element.Debug = debugAttribute.ValueAsBool(element);
            }
        }
    }
}
