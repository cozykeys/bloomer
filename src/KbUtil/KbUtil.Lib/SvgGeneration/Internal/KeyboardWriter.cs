namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class KeyboardWriter : IElementWriter<Keyboard>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Keyboard keyboard)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };
            var groupWriter = new GroupWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, keyboard);
            groupWriter.WriteAttributes(writer, keyboard);
            WriteAttributes(writer, keyboard);

            elementWriter.WriteSubElements(writer, keyboard);
            groupWriter.WriteSubElements(writer, keyboard);
            WriteSubElements(writer, keyboard);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Keyboard keyboard)
        {
        }

        public void WriteSubElements(XmlWriter writer, Keyboard keyboard)
        {
        }
    }
}
