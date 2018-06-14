namespace KbUtil.Lib.Deserialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Keyboard;

    public class KeyboardDataDeserializer
    {
        public static Keyboard Deserialize(XElement xElement)
        {
            var keyboard = new Keyboard();

            DeserializeGroup(xElement, keyboard);

            if(TryGetAttribute(xElement, "Version", out XAttribute versionAttribute))
            {
                keyboard.Version = versionAttribute.ValueAsVersion();
            }

            return keyboard;
        }

        private static void DeserializeKey(XElement keyElement, Key key)
        {
            DeserializeElement(keyElement, key);

            IEnumerable<XElement> legendElements = keyElement
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

        private static void DeserializeSpacer(XElement spacerElement, Spacer spacer)
        {
            DeserializeElement(spacerElement, spacer);
        }

        private static Legend DeserializeLegend(XElement legendElement)
        {
            var legend = new Legend();

            if(TryGetAttribute(legendElement, "HorizontalAlignment", out XAttribute horizontalAlignmentAttribute))
            {
                legend.HorizontalAlignment = horizontalAlignmentAttribute.ValueAsLegendHorizontalAlignment();
            }

            if(TryGetAttribute(legendElement, "VerticalAlignment", out XAttribute verticalAlignmentAttribute))
            {
                legend.VerticalAlignment = verticalAlignmentAttribute.ValueAsLegendVerticalAlignment();
            }

            if(TryGetAttribute(legendElement, "Text", out XAttribute textAttribute))
            {
                legend.Text = textAttribute.ValueAsString();
            }

            if(TryGetAttribute(legendElement, "FontSize", out XAttribute fontSizeAttribute))
            {
                legend.FontSize = fontSizeAttribute.ValueAsFloat();
            }

            return legend;
        }

        private static void DeserializeStack(XElement xElement, Stack stack)
        {
            DeserializeGroup(xElement, stack);

            if(TryGetAttribute(xElement, "Orientation", out XAttribute orientationAttribute))
            {
                stack.Orientation = orientationAttribute.ValueAsStackOrientation();
            }
        }

        private static void DeserializeGroup(XElement xElement, Group group)
        {
            DeserializeElement(xElement, group);

            IEnumerable<XElement> childElements = xElement
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element)
                .Select(node => (XElement)node);

            List<Element> children = childElements
                .Select(childElement => DeserializeChild(group, childElement))
                .ToList();

            group.Children = children;
        }

        private static void DeserializeCase(XElement caseElement, Case @case)
        {
            DeserializeElement(caseElement, @case);

            IEnumerable<XElement> holeElements = caseElement
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element
                    && ((XElement)node).Name == "Hole")
                .Select(node => (XElement)node);

            List<Hole> holes = holeElements
                .Select(childElement => DeserializeHole(@case, childElement))
                .ToList();

            @case.Holes = holes;

            var perimeter = new Perimeter();
            if (TryGetSubElement(caseElement, "Perimeter", out XElement perimeterElement))
            {
                DeserializePerimeter(perimeterElement, perimeter);
            }
            @case.Perimeter = perimeter;
        }

        private static void DeserializePerimeter(XElement perimeterElement, Perimeter perimeter)
        {
            IEnumerable<XElement> sideElements = perimeterElement
                .Nodes()
                .Where(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement)node).Name == "Side")
                .Select(node => (XElement)node);

            List<Side> sides = sideElements
                .Select(childElement => DeserializeSide(perimeter, childElement))
                .ToList();

            perimeter.Sides = sides;

            IEnumerable<XElement> cornerElements = perimeterElement
                .Nodes()
                .Where(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement)node).Name == "Corner")
                .Select(node => (XElement)node);

            List<Corner> corners = cornerElements
                .Select(childElement => DeserializeCorner(perimeter, childElement, sides))
                .ToList();

            perimeter.Corners = corners;
        }

        private static Side DeserializeSide(Element parent, XElement sideElement)
        {
            var side = new Side { Parent = parent };

            DeserializeElement(sideElement, side);

            if (TryGetSubElement(sideElement, "Start", out XElement startElement))
            {
                side.Start = DeserializePoint(side, startElement);
            }

            if (TryGetSubElement(sideElement, "End", out XElement endElement))
            {
                side.End = DeserializePoint(side, endElement);
            }

            return side;
        }

        private static Corner DeserializeCorner(Element parent, XElement cornerElement, IEnumerable<Side> sides)
        {
            throw new NotImplementedException();
        }

        private static Point DeserializePoint(Element parent, XElement pointElement)
        {
            var point = new Point { Parent = parent };

            DeserializeElement(pointElement, point);

            return point;
        }

        private static Hole DeserializeHole(Element parent, XElement holeElement)
        {
            var hole = new Hole { Parent = parent };

            DeserializeElement(holeElement, hole);

            if (TryGetAttribute(holeElement, "Size", out XAttribute sizeAttribute))
            {
                if (Hole.Sizes.ContainsKey(sizeAttribute.ValueAsString()))
                {
                    hole.Size = Hole.Sizes[sizeAttribute.ValueAsString()];
                }
                else
                {
                    hole.Size = sizeAttribute.ValueAsFloat();
                }
            }

            return hole;
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
                case "Spacer":
                    child = new Spacer { Parent = parent };
                    DeserializeSpacer(childElement, (Spacer)child);
                    break;
                case "Case":
                    child = new Case { Parent = parent };
                    DeserializeCase(childElement, (Case)child);
                    break;
                default:
                    throw new Exception();
            }
            return child;
        }

        private static void DeserializeElement(XElement xElement, Element element)
        {
            if(TryGetAttribute(xElement, "Name", out XAttribute nameAttribute))
            {
                element.Name = nameAttribute.ValueAsString();
            }

            if(TryGetAttribute(xElement, "XOffset", out XAttribute xOffsetAttribute))
            {
                element.XOffset = xOffsetAttribute.ValueAsFloat();
            }

            if(TryGetAttribute(xElement, "YOffset", out XAttribute yOffsetAttribute))
            {
                element.YOffset = yOffsetAttribute.ValueAsFloat();
            }

            if(TryGetAttribute(xElement, "Rotation", out XAttribute rotationAttribute))
            {
                element.Rotation = rotationAttribute.ValueAsFloat();
            }

            if(TryGetAttribute(xElement, "Height", out XAttribute heightAttribute))
            {
                // This is a bit hacky but it's easier to set key dimensions via units instead of mm
                string heightString = heightAttribute.ValueAsString();
                if (heightString.EndsWith("u"))
                {
                    element.Height = float.Parse(heightString.Replace("u", string.Empty)) * Constants.KeyDiameterMillimeters1u;
                }
                else
                {
                    element.Height = heightAttribute.ValueAsFloat();
                }
            }

            if(TryGetAttribute(xElement, "Width", out XAttribute widthAttribute))
            {
                // This is a bit hacky but it's easier to set key dimensions via units instead of mm
                string widthString = widthAttribute.ValueAsString();
                if (widthString.EndsWith("u"))
                {
                    element.Width = float.Parse(widthString.Replace("u", string.Empty)) * Constants.KeyDiameterMillimeters1u;
                }
                else
                {
                    element.Width = widthAttribute.ValueAsFloat();
                }
            }

            if(TryGetAttribute(xElement, "Margin", out XAttribute marginAttribute))
            {
                element.Margin = marginAttribute.ValueAsFloat();
            }

            if(TryGetAttribute(xElement, "Debug", out XAttribute debugAttribute))
            {
                element.Debug = debugAttribute.ValueAsBool();
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

            return !(attribute is default(XAttribute));
        }

        private static bool TryGetSubElement(XElement xElement, string elementName, out XElement element)
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
