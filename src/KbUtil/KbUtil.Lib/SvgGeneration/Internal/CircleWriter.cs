namespace KbUtil.Lib.SvgGeneration.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Xml;

    internal class CircleWriter : IElementWriter<Circle>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Circle circle)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, circle);
            WriteAttributes(writer, circle);

            elementWriter.WriteSubElements(writer, circle);
            WriteSubElements(writer, circle);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Circle circle)
        {
        }

        public void WriteSubElements(XmlWriter writer, Circle circle)
        {
            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("circle");
            //writer.WriteAttributeString("id", "TODO");
            writer.WriteAttributeString("r", $"{circle.Size/2}");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            if (GenerationOptions != null && GenerationOptions.EnableVisualSwitchCutouts == true)
            {
                writer.WriteStartElement("circle");
                //writer.WriteAttributeString("id", "TODO");
                writer.WriteAttributeString("r", $"{circle.Size/2}");
                writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
                writer.WriteEndElement();
            }
        }
    }
}
