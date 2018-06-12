namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.Extensions;

    internal class KeyWriter : IElementWriter<Key>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Key key)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, key);
            WriteAttributes(writer, key);

            elementWriter.WriteSubElements(writer, key);
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

        private void WriteSwitchCutoutPath(XmlWriter writer, Key key)
        {
            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("path");
            writer.WriteAttributeString("id", $"{key.Name}SwitchCutoutPonoko");
            writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            if (GenerationOptions != null && GenerationOptions.EnableVisualSwitchCutouts == true)
            {
                writer.WriteStartElement("path");
                writer.WriteAttributeString("id", $"{key.Name}SwitchCutoutVisual");
                writer.WriteAttributeString("d", "m -7,-7 h 14 v 1 h 0.8 v 12 h -0.8 v 1 h -14 v -1 h -0.8 v -12 h 0.8 v -1 h 14");
                writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
                writer.WriteEndElement();
            }
        }

        private void WriteKeycapOverlay(XmlWriter writer, Key key)
        {
            if (GenerationOptions == null || GenerationOptions.EnableKeycapOverlays == false)
            {
                return;
            }

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

        private void WriteKeyLegends(XmlWriter writer, Key key)
        {
            if (GenerationOptions == null || GenerationOptions.EnableKeycapOverlays == false)
            {
                return;
            }

            if (key.Legends == null || !key.Legends.Any())
            {
                return;
            }

            int legendIndex = 0;
            foreach (Legend legend in key.Legends)
            {
                writer.WriteStartElement("text");
                writer.WriteAttributeString("id", $"{key.Name}Legend{legendIndex}");
                writer.WriteAttributeString("text-anchor", "middle");

                var style = new Dictionary<string, string>
                {
                    { "alignment-baseline", "middle" },
                    { "font-size", "12px" },
                    { "font-family", "sans-serif" },
                    { "font-weight", "normal" },
                    { "font-style", "normal" },
                };

                writer.WriteAttributeString("style", style.ToCssStyleString());
                writer.WriteString(legend.Text);
                writer.WriteEndElement(); // </text>

                legendIndex++;
            }
        }
    }
}
