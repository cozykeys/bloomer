namespace KbUtil.Lib.SvgGeneration.Internal.Path
{
    using System.Xml;
    using KbUtil.Lib.Models.Path;
    using System.Collections.Generic;
    using KbUtil.Lib.Extensions;

    internal class PathWriter : IElementWriter<Path>
    {
        private static Dictionary<string, string> _pathStyleVisual = new Dictionary<string, string>
        {
            { "fill", "none" },
            { "stroke", "#0000ff" },
            { "stroke-width", "0.01" },
        };

        private static Dictionary<string, string> _pathStylePonoko = new Dictionary<string, string>
        {
            { "fill", "none" },
            { "stroke", "#0000ff" },
            { "stroke-width", "0.5" },
        };

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
            WritePath(writer, path, _pathStyleVisual);
            WritePath(writer, path, _pathStylePonoko);
        }

        private void WritePath(XmlWriter writer, Path path, Dictionary<string, string> styleDictionary)
        {
            writer.WriteStartElement("path");
            writer.WriteAttributeString("style", styleDictionary.ToCssStyleString());
            writer.WriteAttributeString("d", path.Data);
            writer.WriteEndElement();
        }
    }
}
