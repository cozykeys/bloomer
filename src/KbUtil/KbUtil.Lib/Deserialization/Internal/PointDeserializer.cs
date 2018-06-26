namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class PointDeserializer : IDeserializer<Point>
    {
        public static PointDeserializer Default { get; set; } = new PointDeserializer();

        public void Deserialize(XElement pointElement, Point point)
        {
            ElementDeserializer.Default.Deserialize(pointElement, point);
        }
    }
}
