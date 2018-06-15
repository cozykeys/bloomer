namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class CaseWriter : IElementWriter<Case>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Case @case)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, @case);
            WriteAttributes(writer, @case);

            elementWriter.WriteSubElements(writer, @case);
            WriteSubElements(writer, @case);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Case @case)
        {
        }

        public void WriteSubElements(XmlWriter writer, Case @case)
        {
            var holeWriter = new HoleWriter { GenerationOptions = GenerationOptions };

            foreach(Hole hole in @case.Holes)
            {
                holeWriter.Write(writer, hole);
            }

            var perimeterWriter = new PerimeterWriter { GenerationOptions = GenerationOptions };
            perimeterWriter.Write(writer, @case.Perimeter);
        }
    }
}
