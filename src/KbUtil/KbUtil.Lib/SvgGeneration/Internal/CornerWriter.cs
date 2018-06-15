namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class CornerWriter : IElementWriter<Corner>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Corner corner)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, corner);
            WriteAttributes(writer, corner);

            elementWriter.WriteSubElements(writer, corner);
            WriteSubElements(writer, corner);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Corner corner)
        {
        }

        public void WriteSubElements(XmlWriter writer, Corner corner)
        {
            WriteCornerPath(writer, corner);
        }

        private void WriteCornerPath(XmlWriter writer, Corner corner)
        {
            string pathData = GetPathData(corner);

            // First we write it with the style that Ponoko expects
            writer.WriteStartElement("path");
            writer.WriteAttributeString("d", pathData);
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.01");
            writer.WriteEndElement();

            // Next we write it with a style that is more visually pleasing
            if (GenerationOptions == null || GenerationOptions.EnableVisualSwitchCutouts != true)
            {
                return;
            }

            writer.WriteStartElement("path");
            writer.WriteAttributeString("d", pathData);
            writer.WriteAttributeString("style", "fill:none;stroke:#0000ff;stroke-width:0.5");
            writer.WriteEndElement();
        }

        private static string GetPathData(Corner corner)
        {
            var l1 = new Line { P1 = corner.A.Start, P2 = corner.A.End };
            var l2 = new Line { P1 = corner.B.Start, P2 = corner.B.End };

            float cx = (l2.B - l1.B) / (l1.M - l2.M);
            float cy = l1.M * cx + l1.B;

            float x0 = corner.A.End.XOffset;
            float y0 = corner.A.End.YOffset;
            float x1 = corner.B.Start.XOffset;
            float y1 = corner.B.Start.YOffset;

            float dx = x1 - x0;
            float dy = y1 - y0;

            return $"m {x0} {y0} q {cx - x0} {cy - y0} {dx} {dy}";
        }
    }
}
