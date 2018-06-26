namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class PathWriter : IElementWriter<Path>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Path path)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, path);
            WriteAttributes(writer, path);

            elementWriter.WriteSubElements(writer, path);
            WriteSubElements(writer, path);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Path path)
        {
        }

        public void WriteSubElements(XmlWriter writer, Path path)
        {
            var sideWriter = new SideWriter { GenerationOptions = GenerationOptions };
            foreach (Side side in path.Sides)
            {
                sideWriter.Write(writer, side);
            }

            var cornerWriter = new CornerWriter { GenerationOptions = GenerationOptions };
            foreach (Corner corner in path.Corners)
            {
                cornerWriter.Write(writer, corner);
            }
        }
    }
}
