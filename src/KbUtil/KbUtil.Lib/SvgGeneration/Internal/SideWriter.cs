namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class SideWriter : IElementWriter<Side>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Side side)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, side);
            WriteAttributes(writer, side);

            elementWriter.WriteSubElements(writer, side);
            WriteSubElements(writer, side);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Side side)
        {
        }

        public void WriteSubElements(XmlWriter writer, Side side)
        {
            WriteSidePath(writer, side);
        }

        private void WriteSidePath(XmlWriter writer, Side side)
        {
            float x0 = side.Start.XOffset;
            float y0 = side.Start.YOffset;
            float x1 = side.End.XOffset;
            float y1 = side.End.YOffset;

            float dx = x1 - x0;
            float dy = y1 - y0;

            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{side.Name}SidePonoko");
            writer.WriteAttributeString("d", $"m {x0},{y0} {dx},{dy}");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            if (GenerationOptions == null || GenerationOptions.EnableVisualSwitchCutouts != true)
            {
                return;
            }

            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{side.Name}SideVisual");
            writer.WriteAttributeString("d", $"m {x0},{y0} {dx},{dy}");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
            writer.WriteEndElement();
        }
    }
}
