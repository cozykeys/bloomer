namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class KeyDeserializer : IDeserializer<Key>
    {
        public static KeyDeserializer Default { get; set; } = new KeyDeserializer();

        public void Deserialize(XElement keyElement, Key key)
        {
            ElementDeserializer.Default.Deserialize(keyElement, key);

            DeserializeFill(keyElement, key);
            DeserializeStroke(keyElement, key);
            DeserializeLegends(keyElement, key);
        }

        private void DeserializeFill(XElement keyElement, Key key)
        {
            if(XmlUtilities.TryGetAttribute(keyElement, "Fill", out XAttribute fillAttribute))
            {
                key.Fill = fillAttribute.ValueAsString(key);
            }
        }

        private void DeserializeStroke(XElement keyElement, Key key)
        {
            if(XmlUtilities.TryGetAttribute(keyElement, "Stroke", out XAttribute strokeAttribute))
            {
                key.Stroke = strokeAttribute.ValueAsString(key);
            }
        }

        private void DeserializeLegends(XElement keyElement, Key key)
        {
            IEnumerable<XElement> legendElements = keyElement
                .Nodes()
                .Where(node =>
                    node.NodeType == System.Xml.XmlNodeType.Element
                    && ((XElement)node).Name == "Legend")
                .Select(node => (XElement)node);

            key.Legends = legendElements
                .Select(legendElement => DeserializeLegend(key, legendElement));
        }

        private static Legend DeserializeLegend(Key parent, XElement legendElement)
        {
            var legend = new Legend { Parent = parent };
            LegendDeserializer.Default.Deserialize(legendElement, legend);
            return legend;
        }
    }
}
