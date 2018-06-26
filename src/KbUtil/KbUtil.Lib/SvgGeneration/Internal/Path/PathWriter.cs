namespace KbUtil.Lib.SvgGeneration.Internal.Path
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.Models.Path;
    using System;

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
            throw new NotImplementedException();
        }
    }
}
