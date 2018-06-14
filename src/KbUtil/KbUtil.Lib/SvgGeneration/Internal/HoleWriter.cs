namespace KbUtil.Lib.SvgGeneration.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Xml;

    internal class HoleWriter : IElementWriter<Hole>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Hole hole)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, hole);
            WriteAttributes(writer, hole);

            elementWriter.WriteSubElements(writer, hole);
            WriteSubElements(writer, hole);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Hole hole)
        {
        }

        public void WriteSubElements(XmlWriter writer, Hole hole)
        {
            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("circle");
            //writer.WriteAttributeString("id", "TODO");
            writer.WriteAttributeString("r", $"{hole.Size/2}");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            if (GenerationOptions != null && GenerationOptions.EnableVisualSwitchCutouts == true)
            {
                writer.WriteStartElement("circle");
                //writer.WriteAttributeString("id", "TODO");
                writer.WriteAttributeString("r", $"{hole.Size/2}");
                writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
                writer.WriteEndElement();
            }
        }
    }
}
