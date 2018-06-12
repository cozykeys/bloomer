namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class StackWriter : IElementWriter<Stack>
    {
        private StackWriter()
        {
        }

        public static StackWriter Instance { get; } = new StackWriter();

        public void Write(XmlWriter writer, Stack stack)
        {
            writer.WriteStartElement("g");

            ElementWriter.Instance.WriteAttributes(writer, stack);
            GroupWriter.Instance.WriteAttributes(writer, stack);
            WriteAttributes(writer, stack);

            ElementWriter.Instance.WriteSubElements(writer, stack);
            GroupWriter.Instance.WriteSubElements(writer, stack);
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
