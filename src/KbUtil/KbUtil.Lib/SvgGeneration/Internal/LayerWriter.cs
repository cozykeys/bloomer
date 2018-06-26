namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class LayerWriter : IElementWriter<Layer>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Layer layer)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, layer);
            WriteAttributes(writer, layer);

            elementWriter.WriteSubElements(writer, layer);
            WriteSubElements(writer, layer);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Layer layer)
        {
        }

        public void WriteSubElements(XmlWriter writer, Layer layer)
        {
            WriteGroups(writer, layer);
        }

        private void WriteGroups(XmlWriter writer, Layer layer)
        {
            var groupWriter = new GroupWriter { GenerationOptions = GenerationOptions };
            foreach (var group in layer.Groups)
            {
                groupWriter.Write(writer, group);
            }
        }
    }
}
