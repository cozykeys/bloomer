namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class HoleDeserializer : IDeserializer<Hole>
    {
        public static HoleDeserializer Default { get; set; } = new HoleDeserializer();

        public void Deserialize(XElement holeElement, Hole hole)
        {
            ElementDeserializer.Default.Deserialize(holeElement, hole);

            DeserializeSize(holeElement, hole);
        }

        private void DeserializeSize(XElement holeElement, Hole hole)
        {
            if (XmlUtilities.TryGetAttribute(holeElement, "Size", out XAttribute sizeAttribute))
            {
                hole.Size = sizeAttribute.ValueAsFloat(hole);
            }
        }
    }
}
