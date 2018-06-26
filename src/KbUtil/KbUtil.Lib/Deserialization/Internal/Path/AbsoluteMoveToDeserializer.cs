namespace KbUtil.Lib.Deserialization.Path
{
    using System.Xml.Linq;
    using Models.Path;
    using KbUtil.Lib.Deserialization.Internal;
    using KbUtil.Lib.Deserialization.Internal.Geometry;
    using KbUtil.Lib.Models.Geometry;

    public class AbsoluteMoveToDeserializer : IDeserializer<AbsoluteMoveTo>
    {
        public void Deserialize(XElement absoluteMoveToElement, AbsoluteMoveTo absoluteMoveTo)
        {
            DeserializeEndPoint(absoluteMoveToElement, absoluteMoveTo);
        }

        private void DeserializeEndPoint(XElement absoluteMoveToElement, AbsoluteMoveTo absoluteMoveTo)
        {
            if (XmlUtilities.TryGetSubElement(absoluteMoveToElement, "EndPoint", out XElement endPointElement))
            {
                absoluteMoveTo.EndPoint = new Vec2();
                Vec2Deserializer.Default.Deserialize(endPointElement, absoluteMoveTo.EndPoint);
            }
        }
    }
}
