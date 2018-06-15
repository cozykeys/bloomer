namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class PerimeterWriter : IElementWriter<Perimeter>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Perimeter perimeter)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, perimeter);
            WriteAttributes(writer, perimeter);

            elementWriter.WriteSubElements(writer, perimeter);
            WriteSubElements(writer, perimeter);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Perimeter perimeter)
        {
        }

        public void WriteSubElements(XmlWriter writer, Perimeter perimeter)
        {
            var sideWriter = new SideWriter { GenerationOptions = GenerationOptions };
            foreach (Side side in perimeter.Sides)
            {
                sideWriter.Write(writer, side);
            }

            var cornerWriter = new CornerWriter { GenerationOptions = GenerationOptions };
            foreach (Corner corner in perimeter.Corners)
            {
                cornerWriter.Write(writer, corner);
            }
        }
    }
}
