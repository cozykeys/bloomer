namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class ElementWriter : IElementWriter<Element>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Element element)
        {
            writer.WriteStartElement("g");

            WriteAttributes(writer, element);

            WriteSubElements(writer, element);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Element element)
        {
            writer.WriteAttributeString("id", element.Name);
            WriteTransform(writer, element);
        }

        public void WriteSubElements(XmlWriter writer, Element element)
        {
            if (element.Debug)
            {
                WriteDebugOverlay(writer, element);
            }
        }

        private static void WriteDebugOverlay(XmlWriter writer, Element element)
        {
            // Write border (With Margin)
            {
                float w = element.Width + element.Margin * 2;
                float h = element.Height + element.Margin * 2;

                writer.WriteStartElement("path");
                writer.WriteAttributeString("id", $"{element.Name}DebugOverlayMargin");
                writer.WriteAttributeString("d", $"M -{w/2},-{h/2} h {w} v {h} h -{w} v -{h} h {w}");
                writer.WriteAttributeString("style", "fill:none;stroke:#00ff00;stroke-width:0.1");
                writer.WriteEndElement();
            }

            // Write border (Without Margin)
            {
                float w = element.Width + element.Margin * 2;
                float h = element.Height + element.Margin * 2;

                writer.WriteStartElement("path");
                writer.WriteAttributeString("id", $"{element.Name}DebugOverlayBorder");
                writer.WriteAttributeString("d", $"M -{w/2},-{h/2} h {w} v {h} h -{w} v -{h} h {w}");
                writer.WriteAttributeString("style", "fill:none;stroke:#00ffff;stroke-width:0.1");
                writer.WriteEndElement();
            }

            // Write center axes
            {
                float w = element.Width + element.Margin * 2;
                float h = element.Height + element.Margin * 2;

                writer.WriteStartElement("path");
                writer.WriteAttributeString("id", $"{element.Name}DebugOverlayBorder");
                writer.WriteAttributeString("d", $"m 0,0 h 5 h -10 h 5 v 5 v -10 v 5");
                writer.WriteAttributeString("style", "fill:none;stroke:#00ffff;stroke-width:0.1");
                writer.WriteEndElement();
            }
        }

        private static void WriteTransform(XmlWriter writer, Element element)
        {
            var transformationStrings = new List<string>();

            float xOffset = element.XOffset;
            float yOffset = element.YOffset;
            if (element.Parent is Stack)
            {
                switch (((Stack) element.Parent).Orientation)
                {
                    case StackOrientation.Horizontal:
                        xOffset += GetOffsetInStack((Stack)element.Parent, element);
                        break;
                    case StackOrientation.Vertical:
                        yOffset += GetOffsetInStack((Stack)element.Parent, element);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!(xOffset is default(float)) || !(yOffset is default(float)))
            {
                transformationStrings.Add($"translate({xOffset},{yOffset})");
            }

            if (!(element.Rotation is default(float)))
            {
                transformationStrings.Add($"rotate({element.Rotation})");
            }

            if (transformationStrings.Any())
            {
                writer.WriteAttributeString("transform", string.Join(" ", transformationStrings));
            }
        }

        private static float GetOffsetInStack(Stack stack, Element element)
        {
            switch (stack.Orientation)
            {
                case StackOrientation.Vertical:
                    float stackHeight = default;
                    float dy = float.MaxValue;

                    foreach (Element child in stack.Children)
                    {
                        if (child == element)
                        {
                            dy = stackHeight + child.Height / 2 + child.Margin;
                        }

                        stackHeight += child.Height + child.Margin * 2;
                    }

                    return -(stackHeight / 2 - dy);

                case StackOrientation.Horizontal:
                    float stackWidth = default;
                    float dx = float.MaxValue;

                    foreach (Element child in stack.Children)
                    {
                        if (child == element)
                        {
                            dx = stackWidth + child.Width / 2 + child.Margin;
                        }

                        stackWidth += child.Width + child.Margin * 2;
                    }

                    return -(stackWidth / 2 - dx);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
