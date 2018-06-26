namespace KbUtil.Lib.Deserialization.Path
{
    using System.Xml.Linq;
    using Models.Path;
    using KbUtil.Lib.Deserialization.Internal;
    using KbUtil.Lib.Models.Geometry;
    using KbUtil.Lib.Deserialization.Internal.Geometry;

    public class AbsoluteQuadraticCurveToDeserializer : IDeserializer<AbsoluteQuadraticCurveTo>
    {
        public void Deserialize(XElement absoluteQuadraticCurveToElement, AbsoluteQuadraticCurveTo absoluteQuadraticCurveTo)
        {
            DeserializeEndPoint(absoluteQuadraticCurveToElement, absoluteQuadraticCurveTo);
            DeserializeControlPoint(absoluteQuadraticCurveToElement, absoluteQuadraticCurveTo);
        }

        private void DeserializeEndPoint(XElement absoluteQuadraticCurveToElement, AbsoluteQuadraticCurveTo absoluteQuadraticCurveTo)
        {
            if (XmlUtilities.TryGetSubElement(absoluteQuadraticCurveToElement, "EndPoint", out XElement endPointElement))
            {
                absoluteQuadraticCurveTo.EndPoint = new Vec2();
                Vec2Deserializer.Default.Deserialize(endPointElement, absoluteQuadraticCurveTo.EndPoint);
            }
        }

        private void DeserializeControlPoint(XElement absoluteQuadraticCurveToElement, AbsoluteQuadraticCurveTo absoluteQuadraticCurveTo)
        {
            if (XmlUtilities.TryGetSubElement(absoluteQuadraticCurveToElement, "ControlPoint", out XElement controlPointElement))
            {
                absoluteQuadraticCurveTo.ControlPoint = new Vec2();
                Vec2Deserializer.Default.Deserialize(controlPointElement, absoluteQuadraticCurveTo.ControlPoint);
            }
        }
    }
}
