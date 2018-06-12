namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class StackWriter : IElementWriter<Stack>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Stack stack)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };
            var groupWriter = new GroupWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, stack);
            groupWriter.WriteAttributes(writer, stack);
            WriteAttributes(writer, stack);

            elementWriter.WriteSubElements(writer, stack);
            groupWriter.WriteSubElements(writer, stack);
            WriteSubElements(writer, stack);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Stack stack)
        {
        }

        public void WriteSubElements(XmlWriter writer, Stack stack)
        {
        }
    }
}
