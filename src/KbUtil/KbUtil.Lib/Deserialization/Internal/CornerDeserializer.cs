namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.IO;
    using System.Linq;

    internal class CornerDeserializer : IDeserializer<Corner>
    {
        public static CornerDeserializer Default { get; set; } = new CornerDeserializer();

        public void Deserialize(XElement cornerElement, Corner corner)
        {
            ElementDeserializer.Default.Deserialize(cornerElement, corner);

            IEnumerable<Side> sides = ((Models.Keyboard.Path)corner.Parent).Sides;

            DeserializeSideA(cornerElement, corner, sides);
            DeserializeSideB(cornerElement, corner, sides);
        }

        private void DeserializeSideA(XElement cornerElement, Corner corner, IEnumerable<Side> sides)
        {
            corner.A = DeserializeSide(cornerElement, corner, sides, "SideA");
        }

        private void DeserializeSideB(XElement cornerElement, Corner corner, IEnumerable<Side> sides)
        {
            corner.B = DeserializeSide(cornerElement, corner, sides, "SideB");
        }

        private Side DeserializeSide(XElement cornerElement, Corner corner, IEnumerable<Side> sides, string elementName)
        {
            string sideName = string.Empty;
            if (XmlUtilities.TryGetAttribute(cornerElement, elementName, out XAttribute sideAAttribute))
            {
                sideName = sideAAttribute.ValueAsString(corner);
            }

            if (string.IsNullOrEmpty(sideName))
            {
                throw new InvalidDataException();
            }

            return sides.Single(side => side.Name == sideName);
        }
    }
}
