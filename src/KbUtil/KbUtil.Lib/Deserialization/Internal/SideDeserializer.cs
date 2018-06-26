namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class SideDeserializer : IDeserializer<Side>
    {
        public static SideDeserializer Default { get; set; } = new SideDeserializer();

        public void Deserialize(XElement sideElement, Side side)
        {
            ElementDeserializer.Default.Deserialize(sideElement, side);

            DeserializeStart(sideElement, side);
            DeserializeEnd(sideElement, side);
        }

        private void DeserializeStart(XElement sideElement, Side side)
        {
            if (XmlUtilities.TryGetSubElement(sideElement, "Start", out XElement startElement))
            {
                side.Start = new Point { Parent = side };
                PointDeserializer.Default.Deserialize(startElement, side.Start);
            }
        }

        private void DeserializeEnd(XElement sideElement, Side side)
        {
            if (XmlUtilities.TryGetSubElement(sideElement, "End", out XElement endElement))
            {
                side.End = new Point { Parent = side };
                PointDeserializer.Default.Deserialize(endElement, side.End);
            }
        }
    }
}
