namespace KbUtil.Lib.Deserialization.Internal.Geometry
{
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Geometry;
    using KbUtil.Lib.Deserialization.Extensions;

    internal class Vec2Deserializer : IDeserializer<Vec2>
    {
        public static Vec2Deserializer Default { get; set; } = new Vec2Deserializer();

        public void Deserialize(XElement vec2Element, Vec2 vec2)
        {
            DeserializeX(vec2Element, vec2);
            DeserializeY(vec2Element, vec2);
        }

        private void DeserializeX(XElement vec2Element, Vec2 vec2)
        {
            if(XmlUtilities.TryGetAttribute(vec2Element, "X", out XAttribute xAttribute))
            {
                vec2.X = xAttribute.ValueAsFloat();
            }
        }

        private void DeserializeY(XElement vec2Element, Vec2 vec2)
        {
            if(XmlUtilities.TryGetAttribute(vec2Element, "Y", out XAttribute yAttribute))
            {
                vec2.Y = yAttribute.ValueAsFloat();
            }
        }
    }
}
