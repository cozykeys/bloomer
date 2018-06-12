namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class KeyWriter : IElementWriter<Key>
    {
        private KeyWriter()
        {
        }

        public static KeyWriter Instance { get; } = new KeyWriter();

        public void Write(XmlWriter writer, Key key)
        {
            writer.WriteStartElement("g");

            // Attributes
            ElementWriter.Instance.WriteAttributes(writer, key);
            WriteAttributes(writer, key);

            // Elements
            ElementWriter.Instance.WriteSubElements(writer, key);
            WriteSubElements(writer, key);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Key key)
        {
        }

        public void WriteSubElements(XmlWriter writer, Key key)
        {
            WriteSwitchCutoutPath(writer, key);
            WriteKeycapOverlay(writer, key);
            WriteKeyLegends(writer, key);
        }

        private static void WriteSwitchCutoutPath(XmlWriter writer, Key key)
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

        private static void WriteKeycapOverlay(XmlWriter writer, Key key)
        {
            // Give these short names so the resulting path data is readable
            float w = key.Width;
            float h = key.Height;

            // Next we write it with a style that is more visually pleasing
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{key.Name}KeycapOverlay");
            writer.WriteAttributeString("d", $"M -{w/2},-{h/2} h {w} v {h} h -{w} v -{h} h {w}");
            writer.WriteAttributeString("style", "fill:none;stroke:#ff0000;stroke-width:0.5");
            writer.WriteEndElement();
        }

        private static void WriteKeyLegends(XmlWriter writer, Key key)
        {
            if (key.Legends == null || !key.Legends.Any())
            {
                return;
            }

            int legendIndex = 0;
            foreach (Legend legend in key.Legends)
            {
                writer.WriteStartElement("text");
                writer.WriteAttributeString("id", $"{key.Name}Legend{legendIndex}");

                var style = new List<string>
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

                writer.WriteString(legend.Text);

                writer.WriteEndElement(); // </tspan>

                writer.WriteEndElement(); // </text>

                legendIndex++;
            }
        }
    }
}
