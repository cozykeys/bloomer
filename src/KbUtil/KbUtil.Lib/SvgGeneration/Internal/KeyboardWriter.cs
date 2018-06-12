namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class KeyboardWriter : IElementWriter<Keyboard>
    {
        private KeyboardWriter()
        {
        }

        public static KeyboardWriter Instance { get; } = new KeyboardWriter();

        public void Write(XmlWriter writer, Keyboard keyboard)
        {
            writer.WriteStartElement("g");

            // Attributes
            ElementWriter.Instance.WriteAttributes(writer, keyboard);
            GroupWriter.Instance.WriteAttributes(writer, keyboard);
            WriteAttributes(writer, keyboard);

            // Elements
            ElementWriter.Instance.WriteSubElements(writer, keyboard);
            GroupWriter.Instance.WriteSubElements(writer, keyboard);
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
