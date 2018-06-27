namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.Extensions;
    using KbUtil.Lib.SvgGeneration.Internal.Path;

    internal class KeyWriter : IElementWriter<Key>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        private static Models.Path.Path _switchPath = new Models.Path.Path
        {
            Components = new List<Models.Path.IPathComponent>
            {
                new Models.Path.AbsoluteMoveTo { EndPoint = new Models.Geometry.Vec2 { X = -7, Y = -7 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7, Y = -7 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7, Y = -6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7.8f, Y = -6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7.8f, Y = 6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7, Y = 6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = 7, Y = 7 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7, Y = 7 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7, Y = 6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7.8f, Y = 6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7.8f, Y = -6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7, Y = -6 } },
                new Models.Path.AbsoluteLineTo { EndPoint = new Models.Geometry.Vec2 { X = -7, Y = -7 } }
            }
        };

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
            var pathWriter = new PathWriter { GenerationOptions = GenerationOptions };
            pathWriter.Write(writer, _switchPath);
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
            writer.WriteAttributeString("d", $"M -{w / 2},-{h / 2} h {w} v {h} h -{w} v -{h} h {w}");

            var styleDictionary = new Dictionary<string, string>
            {
                { "fill", !string.IsNullOrWhiteSpace(key.Fill) ? key.Fill : "#ffffff" },
                { "stroke", !string.IsNullOrWhiteSpace(key.Stroke) ? key.Stroke : "#000000" },
                { "stroke-width", "0.5" },
            };

            writer.WriteAttributeString("style", styleDictionary.ToCssStyleString());
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

                float fontSize = legend.FontSize is default(float) ? 4 : legend.FontSize;

                var styleDictionary = new Dictionary<string, string>
                {
                    { "fill", !string.IsNullOrWhiteSpace(legend.Color) ? legend.Color : "#000000" },
                    { "dominant-baseline", "central" },
                    { "text-anchor", "middle" },
                    { "font-size", $"{fontSize}px" },
                    { "font-family", "sans-serif" },
                    { "font-weight", "normal" },
                    { "font-style", "normal" },
                };

                writer.WriteAttributeString("style", styleDictionary.ToCssStyleString());
                writer.WriteString(legend.Text);
                writer.WriteEndElement(); // </text>

                legendIndex++;
            }
        }
    }
}
