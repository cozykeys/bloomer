namespace KbUtil.Console.Services.Concrete
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    internal class SvgService : ISvgService
    {
        private const string Indentation = "    ";

        private readonly IEnvironmentService _environmentService;

        public SvgService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public void GenerateSvg(KbElementKeyboard kbElementKeyboard, string path)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = Indentation,
                NewLineOnAttributes = true,
                NewLineChars = _environmentService.NewLine()
            };

            using (FileStream stream = File.Open(path, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                WriteSvgOpenTag(writer);

                WriteKbElementKeyboard(writer, kbElementKeyboard);

                WriteSvgCloseTag(writer);
            }
        }

        private void WriteSvgOpenTag(XmlWriter writer)
        {
            writer.WriteStartElement("svg", "http://www.w3.org/2000/svg");
            writer.WriteAttributeString("width", "500mm");
            writer.WriteAttributeString("height", "500mm");
            writer.WriteAttributeString("viewBox", "0 0 500 500");
        }

        private void WriteSvgCloseTag(XmlWriter writer)
        {
            writer.WriteEndElement();
        }

        private void WriteKbElementKeyboard(XmlWriter writer, KbElementKeyboard kbElementKeyboard)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteKbElementAttributes(writer, kbElementKeyboard);
            WriteKbElementGroupAttributes(writer, kbElementKeyboard);
            WriteKbElementKeyboardAttributes(writer, kbElementKeyboard);

            // Elements
            WriteKbElementSubElements(writer, kbElementKeyboard);
            WriteKbElementGroupSubElements(writer, kbElementKeyboard);
            WriteKbElementKeyboardSubElements(writer, kbElementKeyboard);

            writer.WriteEndElement();
        }

        private void WriteKbElementKeyboardAttributes(XmlWriter writer, KbElementKeyboard kbElementKeyboard)
        {
        }

        private void WriteKbElementKeyboardSubElements(XmlWriter writer, KbElementKeyboard kbElementKeyboard)
        {
        }

        private void WriteKbElementGroup(XmlWriter writer, KbElementGroup kbElementGroup)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteKbElementAttributes(writer, kbElementGroup);
            WriteKbElementGroupAttributes(writer, kbElementGroup);

            // Elements
            WriteKbElementSubElements(writer, kbElementGroup);
            WriteKbElementGroupSubElements(writer, kbElementGroup);

            writer.WriteEndElement();
        }

        private void WriteKbElementGroupAttributes(XmlWriter writer, KbElementGroup kbElementGroup)
        {
        }

        private void WriteKbElementGroupSubElements(XmlWriter writer, KbElementGroup kbElementGroup)
        {
            foreach (KbElement child in kbElementGroup.Children)
            {
                switch (child)
                {
                    case var _ when child is KbElementKeyboard:
                        throw new InvalidDataException("KbElementKeyboard is not a valid child type.");
                    case var kbElementKey when child is KbElementKey:
                        WriteKbElementKey(writer, (KbElementKey)kbElementKey);
                        break;
                    case var kbElementSubGroup when child is KbElementGroup:
                        WriteKbElementGroup(writer, (KbElementGroup)kbElementSubGroup);
                        break;
                    case var kbElement when child is KbElement:
                        WriteKbElement(writer, kbElement);
                        break;
                }
            }
        }

        private void WriteKbElementKey(XmlWriter writer, KbElementKey kbElementKey)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteKbElementAttributes(writer, kbElementKey);
            WriteKbElementKeyAttributes(writer, kbElementKey);

            // Elements
            WriteKbElementSubElements(writer, kbElementKey);
            WriteKbElementKeySubElements(writer, kbElementKey);

            writer.WriteEndElement();
        }

        private void WriteKbElementKeyAttributes(XmlWriter writer, KbElementKey kbElementKey)
        {
        }

        private void WriteKbElementKeySubElements(XmlWriter writer, KbElementKey kbElementKey)
        {
            WriteSwitchCutoutPath(writer, kbElementKey);
            WriteKeycapOverlay(writer, kbElementKey);
        }

        private void WriteKbElement(XmlWriter writer, KbElement kbElement)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteKbElementAttributes(writer, kbElement);

            // Elements
            WriteKbElementSubElements(writer, kbElement);

            writer.WriteEndElement();
        }

        private void WriteKbElementAttributes(XmlWriter writer, KbElement kbElement)
        {
            writer.WriteAttributeString("id", kbElement.Name);
            WriteTransform(writer, kbElement);
        }

        private void WriteKbElementSubElements(XmlWriter writer, KbElement kbElement)
        {
        }

        private void WriteTransform(XmlWriter writer, KbElement kbElement)
        {
            List<string> transformationStrings = new List<string>();

            if (!(kbElement.Rotation is default))
            {
                transformationStrings.Add($"rotate({kbElement.Rotation})");
            }

            if (!(kbElement.XOffset is default) || !(kbElement.YOffset is default))
            {
                transformationStrings.Add($"translate({kbElement.XOffset},{kbElement.YOffset})");
            }

            if (transformationStrings.Any())
            {
                writer.WriteAttributeString("transform", string.Join(";", transformationStrings));
            }
        }

        private void WriteSwitchCutoutPath(XmlWriter writer, KbElementKey kbElementKey)
        {
            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{kbElementKey.Name}SwitchCutoutPonoko");
            writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{kbElementKey.Name}SwitchCutoutVisual");
            writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
            writer.WriteEndElement();
        }

        private void WriteKeycapOverlay(XmlWriter writer, KbElementKey kbElementKey)
        {
            const float KeycapDiameterMm1u = 18.1f;

            // Give these short names so the resulting path data is readable
            float w = kbElementKey.Width * KeycapDiameterMm1u; // Overlay Width
            float h = kbElementKey.Height * KeycapDiameterMm1u; // Overlay Height

            // Next we write it with a style that is more visually pleasing
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{kbElementKey.Name}KeycapOverlay");
            writer.WriteAttributeString("d", $"M -{w/2},-{h/2} h {w} v {h} h -{w} v -{h} h {w}");
            writer.WriteAttributeString("style", "fill:none;stroke:#ff0000;stroke-width:0.5");
            writer.WriteEndElement();
        }
    }
}
