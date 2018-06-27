namespace KbUtil.Lib.Deserialization.Path
{
    using System.Xml.Linq;
    using Models.Path;
    using KbUtil.Lib.Deserialization.Internal;
    using KbUtil.Lib.Models.Geometry;
    using KbUtil.Lib.Deserialization.Internal.Geometry;

    public class AbsoluteLineToDeserializer : IDeserializer<AbsoluteLineTo>
    {
        public void Deserialize(XElement absoluteLineToElement, AbsoluteLineTo absoluteLineTo)
        {
            DeserializeEndPoint(absoluteLineToElement, absoluteLineTo);
        }

        private void DeserializeEndPoint(XElement absoluteLineToElement, AbsoluteLineTo absoluteLineTo)
        {
            if (XmlUtilities.TryGetSubElement(absoluteLineToElement, "EndPoint", out XElement endPointElement))
            {
                absoluteLineTo.EndPoint = new Vec2();
                Vec2Deserializer.Default.Deserialize(endPointElement, absoluteLineTo.EndPoint);
            }
        }
    }
}
