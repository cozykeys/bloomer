namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    internal class PathDeserializer : IDeserializer<Path>
    {
        public static PathDeserializer Default { get; set; } = new PathDeserializer();

        public void Deserialize(XElement pathElement, Path path)
        {
            ElementDeserializer.Default.Deserialize(pathElement, path);

            DeserializeSides(pathElement, path);
            DeserializeCorners(pathElement, path);
        }

        private void DeserializeSides(XElement pathElement, Path path)
        {
            IEnumerable<XElement> sideElements = pathElement
                .Nodes()
                .Where(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement)node).Name == "Side")
                .Select(node => (XElement)node);

            List<Side> sides = sideElements
                .Select(sideElement => DeserializeSide(path, sideElement))
                .ToList();

            path.Sides = sides;
        }

        private Side DeserializeSide(Path parent, XElement sideElement)
        {
            var side = new Side { Parent = parent };
            SideDeserializer.Default.Deserialize(sideElement, side);
            return side;
        }

        private void DeserializeCorners(XElement pathElement, Path path)
        {
            IEnumerable<XElement> cornerElements = pathElement
                .Nodes()
                .Where(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement)node).Name == "Corner")
                .Select(node => (XElement)node);

            List<Corner> corners = cornerElements
                .Select(cornerElement => DeserializeCorner(path, cornerElement))
                .ToList();

            path.Corners = corners;
        }

        private Corner DeserializeCorner(Path parent, XElement cornerElement)
        {
            var corner = new Corner { Parent = parent };
            CornerDeserializer.Default.Deserialize(cornerElement, corner);
            return corner;
        }
    }
}
