namespace KbUtil.Lib.SvgGeneration
{
    using System.IO;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.SvgGeneration.Internal;

    public class SvgGenerator
    {
        public static void GenerateSvg(Keyboard keyboard, string path, SvgGenerationOptions options = null)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = options?.IndentString ?? "  ",
                NewLineOnAttributes = true
            };

            using (FileStream stream = File.Open(path, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                WriteSvgOpenTag(writer);

                var keyboardWriter = new KeyboardWriter { GenerationOptions = options };
                keyboardWriter.Write(writer, keyboard);

                WriteSvgCloseTag(writer);
            }
        }

        private static void WriteSvgOpenTag(XmlWriter writer)
        {
            writer.WriteStartElement("svg", "http://www.w3.org/2000/svg");
            writer.WriteAttributeString("width", "500mm");
            writer.WriteAttributeString("height", "500mm");
            writer.WriteAttributeString("viewBox", "0 0 500 500");
        }

        private static void WriteSvgCloseTag(XmlWriter writer)
        {
            writer.WriteEndElement();
        }
    }
}
