using KbUtil.Lib.Models.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KbUtil.Console.Deserializers
{
    class XmlDeserializer
    {
        public static Keyboard DeserializeKeyboard(XElement xElement)
        {
            Keyboard Keyboard = new Keyboard();

            DeserializeGroup(xElement, Keyboard);

            Keyboard.Version = default;
            if(TryGetAttribute(xElement, "Version", out XAttribute versionAttribute))
            {
                Keyboard.Version = versionAttribute.ValueAsVersion();
            }

            return Keyboard;
        }

        private static void DeserializeKey(XElement keyElement, Key key)
        {
            DeserializeElement(keyElement, key);

            var legendElements = keyElement
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element
                    && ((XElement)node).Name == "Legend")
                .Select(node => (XElement)node);

            var legends = new List<Legend>();
            foreach (var legendElement in legendElements)
            {
                legends.Add(DeserializeLegend(legendElement));
            }
            key.Legends = legends;
        }

        private static Legend DeserializeLegend(XElement legendElement)
        {
            var legend = new Legend();

            legend.HorizontalAlignment = default;
            if(TryGetAttribute(legendElement, "HorizontalAlignment", out XAttribute horizontalAlignmentAttribute))
            {
                legend.HorizontalAlignment = horizontalAlignmentAttribute.ValueAsLegendHorizontalAlignment();
            }

            legend.VerticalAlignment = default;
            if(TryGetAttribute(legendElement, "VerticalAlignment", out XAttribute verticalAlignmentAttribute))
            {
                legend.VerticalAlignment = verticalAlignmentAttribute.ValueAsLegendVerticalAlignment();
            }

            legend.Text = default;
            if(TryGetAttribute(legendElement, "Text", out XAttribute textAttribute))
            {
                legend.Text = textAttribute.ValueAsString();
            }

            legend.FontSize = default;
            if(TryGetAttribute(legendElement, "FontSize", out XAttribute fontSizeAttribute))
            {
                legend.FontSize = fontSizeAttribute.ValueAsFloat();
            }

            return legend;
        }

        private static void DeserializeStack(XElement xElement, Stack stack)
        {
            DeserializeGroup(xElement, stack);

            stack.Orientation = default;
            if(TryGetAttribute(xElement, "Orientation", out XAttribute orientationAttribute))
            {
                stack.Orientation = orientationAttribute.ValueAsStackOrientation();
            }
        }

        private static void DeserializeGroup(XElement xElement, Group group)
        {
            DeserializeElement(xElement, group);

            var children = new List<Element>();

            var childElements = xElement
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element)
                .Select(node => (XElement)node);

            foreach (var childElement in childElements)
            {
                children.Add(DeserializeChild(group, childElement));
            }

            group.Children = children;
        }

        private static Element DeserializeChild(Element parent, XElement childElement)
        {
            Element child;

            switch (childElement.Name.ToString())
            {
                case "Stack":
                    child = new Stack { Parent = parent };
                    DeserializeStack(childElement, (Stack)child);
                    break;
                case "Group":
                    child = new Group { Parent = parent };
                    DeserializeGroup(childElement, (Group)child);
                    break;
                case "Key":
                    child = new Key { Parent = parent };
                    DeserializeKey(childElement, (Key)child);
                    break;
                default:
                    throw new Exception();
            }
            return child;
        }

        private static void DeserializeElement(XElement xElement, Element element)
        {
            element.Name = default;
            if(TryGetAttribute(xElement, "Name", out XAttribute nameAttribute))
            {
                element.Name = nameAttribute.ValueAsString();
            }

            element.XOffset = default;
            if(TryGetAttribute(xElement, "XOffset", out XAttribute xOffsetAttribute))
            {
                element.XOffset = xOffsetAttribute.ValueAsFloat();
            }

            element.YOffset = default;
            if(TryGetAttribute(xElement, "YOffset", out XAttribute yOffsetAttribute))
            {
                element.YOffset = yOffsetAttribute.ValueAsFloat();
            }

            element.Rotation = default;
            if(TryGetAttribute(xElement, "Rotation", out XAttribute rotationAttribute))
            {
                element.Rotation = rotationAttribute.ValueAsFloat();
            }

            element.Height = default;
            if(TryGetAttribute(xElement, "Height", out XAttribute heightAttribute))
            {
                // This is a bit hacky but it's easier to set key dimensions via units instead of mm
                var heightString = heightAttribute.ValueAsString();
                if (heightString.EndsWith("u"))
                {
                    element.Height = float.Parse(heightString.Replace("u", string.Empty)) * Constants.KeyDiameterMillimeters1u;
                }
                else
                {
                    element.Height = heightAttribute.ValueAsFloat();
                }
            }

            element.Width = default;
            if(TryGetAttribute(xElement, "Width", out XAttribute widthAttribute))
            {
                // This is a bit hacky but it's easier to set key dimensions via units instead of mm
                var widthString = widthAttribute.ValueAsString();
                if (widthString.EndsWith("u"))
                {
                    element.Width = float.Parse(widthString.Replace("u", string.Empty)) * Constants.KeyDiameterMillimeters1u;
                }
                else
                {
                    element.Width = widthAttribute.ValueAsFloat();
                }
            }

            element.Margin = default;
            if(TryGetAttribute(xElement, "Margin", out XAttribute marginAttribute))
            {
                element.Margin = marginAttribute.ValueAsFloat();
            }
        }

        private static bool TryGetAttribute(XElement xElement, string attributeName, out XAttribute attribute)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(attributeName))
            {
                attribute = null;
                return false;
            }

            attribute = xElement
                .Attributes()
                .FirstOrDefault(attr => attr.Name.ToString() == attributeName);

            return !(attribute is default);
        }

        private static bool TryGetSubElement(XElement xElement, string subElementName, out XElement element)
        {
            if (xElement == null || string.IsNullOrWhiteSpace(subElementName))
            {
                element = null;
                return false;
            }

            element = (XElement)xElement
                .Nodes()
                .FirstOrDefault(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element
                    && ((XElement)node).Name == subElementName);

            return !(element is default);
        }
    }
}
