using KbUtil.Lib.Models.Keyboard;
using System.Xml;

namespace KbUtil.Lib.SvgGeneration.Internal
{
    internal class SpacerWriter : IElementWriter<Spacer>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Spacer spacer)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, spacer);
            WriteAttributes(writer, spacer);

            elementWriter.WriteSubElements(writer, spacer);
            WriteSubElements(writer, spacer);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Spacer keyboard)
        {
        }

        public void WriteSubElements(XmlWriter writer, Spacer keyboard)
        {
        }
    }
}
