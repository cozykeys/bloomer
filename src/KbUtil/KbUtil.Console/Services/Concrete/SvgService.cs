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

        public void GenerateSvg(Keyboard keyboard, string path)
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

                WriteKeyboard(writer, keyboard);

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

        private void WriteKeyboard(XmlWriter writer, Keyboard keyboard)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteElementAttributes(writer, keyboard);
            WriteGroupAttributes(writer, keyboard);
            WriteKeyboardAttributes(writer, keyboard);

            // Elements
            WriteElementSubElements(writer, keyboard);
            WriteGroupSubElements(writer, keyboard);
            WriteKeyboardSubElements(writer, keyboard);

            writer.WriteEndElement();
        }

        private void WriteKeyboardAttributes(XmlWriter writer, Keyboard keyboard)
        {
        }

        private void WriteKeyboardSubElements(XmlWriter writer, Keyboard keyboard)
        {
        }

        private void WriteGroup(XmlWriter writer, Group group)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteElementAttributes(writer, group);
            WriteGroupAttributes(writer, group);

            // Elements
            WriteElementSubElements(writer, group);
            WriteGroupSubElements(writer, group);

            writer.WriteEndElement();
        }

        private void WriteGroupAttributes(XmlWriter writer, Group group)
        {
        }

        private void WriteGroupSubElements(XmlWriter writer, Group group)
        {
            foreach (Element child in group.Children)
            {
                switch (child)
                {
                    case var _ when child is Keyboard:
                        throw new InvalidDataException("Keyboard is not a valid child type.");
                    case var key when child is Key:
                        WriteKey(writer, (Key)key);
                        break;
                    case var subGroup when child is Group:
                        WriteGroup(writer, (Group)subGroup);
                        break;
                    case var element when child is Element:
                        WriteElement(writer, element);
                        break;
                }
            }
        }

        private void WriteKey(XmlWriter writer, Key key)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteElementAttributes(writer, key);
            WriteKeyAttributes(writer, key);

            // Elements
            WriteElementSubElements(writer, key);
            WriteKeySubElements(writer, key);

            writer.WriteEndElement();
        }

        private void WriteKeyAttributes(XmlWriter writer, Key key)
        {
        }

        private void WriteKeySubElements(XmlWriter writer, Key key)
        {
            WriteSwitchCutoutPath(writer, key);
            WriteKeycapOverlay(writer, key);
            WriteKeyLegends(writer, key);
        }

        private void WriteElement(XmlWriter writer, Element element)
        {
            writer.WriteStartElement("g");

            // Attributes
            WriteElementAttributes(writer, element);

            // Elements
            WriteElementSubElements(writer, element);

            writer.WriteEndElement();
        }

        private void WriteElementAttributes(XmlWriter writer, Element element)
        {
            writer.WriteAttributeString("id", element.Name);
            WriteTransform(writer, element);
        }

        private void WriteElementSubElements(XmlWriter writer, Element element)
        {
        }

        private void WriteTransform(XmlWriter writer, Element element)
        {
            List<string> transformationStrings = new List<string>();

            if (!(element.Rotation is default))
            {
                transformationStrings.Add($"rotate({element.Rotation})");
            }

            if (!(element.XOffset is default) || !(element.YOffset is default))
            {
                transformationStrings.Add($"translate({element.XOffset},{element.YOffset})");
            }

            if (transformationStrings.Any())
            {
                writer.WriteAttributeString("transform", string.Join(" ", transformationStrings));
            }
        }

        private void WriteSwitchCutoutPath(XmlWriter writer, Key key)
        {
            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{key.Name}SwitchCutoutPonoko");
            writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{key.Name}SwitchCutoutVisual");
            writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
            writer.WriteEndElement();
        }

        private void WriteKeycapOverlay(XmlWriter writer, Key key)
        {
            const float KeycapDiameterMm1u = 18.1f;

            // Give these short names so the resulting path data is readable
            float w = key.Width * KeycapDiameterMm1u; // Overlay Width
            float h = key.Height * KeycapDiameterMm1u; // Overlay Height

            // Next we write it with a style that is more visually pleasing
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{key.Name}KeycapOverlay");
            writer.WriteAttributeString("d", $"M -{w/2},-{h/2} h {w} v {h} h -{w} v -{h} h {w}");
            writer.WriteAttributeString("style", "fill:none;stroke:#ff0000;stroke-width:0.5");
            writer.WriteEndElement();
        }

        private void WriteKeyLegends(XmlWriter writer, Key key)
        {
            int legendIndex = 0;
            foreach (Legend legend in key.Legends)
            {
                writer.WriteStartElement("text");
                writer.WriteAttributeString("id", $"{key.Name}Legend{legendIndex}");

                List<string> style = new List<string>
                {
                    "font-style:normal",
                    "font-weight:normal",
                    $"font-size:{legend.FontSize}px",
                    "line-height:1.25",
                    "font-family:sans-serif",
                    "letter-spacing:0px",
                    "word-spacing:0px",
                    "fill:#000000",
                    "fill-opacity:1",
                    "stroke:none",
                    "stroke-width:0.26458332"
                };
                writer.WriteAttributeString("style", string.Join(";", style));
                //writer.WriteAttributeString("transform", $"translate(0,{0 - (legend.FontSize / 2)})");

                writer.WriteStartElement("tspan");
                writer.WriteAttributeString("id", $"{key.Name}Legend{legendIndex}tspan");
                writer.WriteAttributeString("style", "stroke-width:0.26458332");

                // todo: How to write text between the tags
                writer.WriteString(legend.Text);

                writer.WriteEndElement(); // </tspan>

                writer.WriteEndElement(); // </text>

                legendIndex++;
            }
        }
    }
}
