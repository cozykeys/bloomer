using KbUtil.Lib.Models.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KbUtil.Console.Deserializers
{
    class XmlDeserializer
    {
        public static KbElementKeyboard DeserializeKbElementKeyboard(XElement element)
        {
            KbElementKeyboard kbElementKeyboard = new KbElementKeyboard();

            DeserializeKbElementGroup(element, kbElementKeyboard);

            if(TryGetAttribute(element, "Version", out XAttribute versionAttribute))
            {
                kbElementKeyboard.Version = versionAttribute.VersionValue();
            }
            else
            {
                kbElementKeyboard.Version = default;
            }

            return kbElementKeyboard;
        }

        private static void DeserializeKbElementKey(XElement element, KbElementKey kbElementKey)
        {
            DeserializeKbElement(element, kbElementKey);

            if(TryGetAttribute(element, "Height", out XAttribute heightAttribute))
            {
                kbElementKey.Height = heightAttribute.FloatValue();
            }
            else
            {
                kbElementKey.Height = default;
            }

            if(TryGetAttribute(element, "Width", out XAttribute widthAttribute))
            {
                kbElementKey.Width = widthAttribute.FloatValue();
            }
            else
            {
                kbElementKey.Width = default;
            }
        }

        private static void DeserializeKbElementGroup(XElement element, KbElementGroup kbElementGroup)
        {
            DeserializeKbElement(element, kbElementGroup);

            if(TryGetSubElement(element, "Children", out XElement childrenElement))
            {
                kbElementGroup.Children = DeserializeChildren(childrenElement);
            }
            else
            {
                kbElementGroup.Children = new List<KbElement>();
            }
        }

        private static IEnumerable<KbElement> DeserializeChildren(XElement element)
        {
            var childrenElements = element
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element)
                .Select(node => (XElement)node);

            List<KbElement> children = new List<KbElement>();
            foreach (var childElement in childrenElements)
            {
                KbElement child;
                switch (childElement.Name.ToString())
                {
                    case "Group":
                        child = new KbElementGroup();
                        DeserializeKbElementGroup(childElement, (KbElementGroup)child);
                        break;
                    case "Key":
                        child = new KbElementKey();
                        DeserializeKbElementKey(childElement, (KbElementKey)child);
                        break;
                    default:
                        throw new Exception();
                }
                children.Add(child);
            }

            return children;
        }

        private static void DeserializeKbElement(XElement element, KbElement kbElement)
        {
            kbElement.Name = default;
            if(TryGetAttribute(element, "Name", out XAttribute nameAttribute))
            {
                kbElement.Name = nameAttribute.StringValue();
            }

            kbElement.XOffset = default;
            if(TryGetAttribute(element, "XOffset", out XAttribute xOffsetAttribute))
            {
                kbElement.XOffset = xOffsetAttribute.FloatValue();
            }

            kbElement.YOffset = default;
            if(TryGetAttribute(element, "YOffset", out XAttribute yOffsetAttribute))
            {
                kbElement.YOffset = yOffsetAttribute.FloatValue();
            }

            kbElement.Rotation = default;
            if(TryGetAttribute(element, "Rotation", out XAttribute rotationAttribute))
            {
                kbElement.Rotation = rotationAttribute.FloatValue();
            }

        }

        private static bool TryGetAttribute(XElement element, string attributeName, out XAttribute attribute)
        {
            if (element == null || string.IsNullOrWhiteSpace(attributeName))
            {
                attribute = null;
                return false;
            }

            attribute = element
                .Attributes()
                .FirstOrDefault(attr => attr.Name.ToString() == attributeName);

            return !(attribute is default);
        }

        private static bool TryGetSubElement(XElement parentElement, string subElementName, out XElement element)
        {
            if (parentElement == null || string.IsNullOrWhiteSpace(subElementName))
            {
                element = null;
                return false;
            }

            element = (XElement)parentElement
                .Nodes()
                .FirstOrDefault(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element
                    && ((XElement)node).Name == subElementName);

            return !(element is default);
        }
    }
}
