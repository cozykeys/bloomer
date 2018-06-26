namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class KeyboardDeserializer : IDeserializer<Keyboard>
    {
        public static KeyboardDeserializer Default { get; set; } = new KeyboardDeserializer();

        public void Deserialize(XElement keyboardElement, Keyboard keyboard)
        {
            ElementDeserializer.Default.Deserialize(keyboardElement, keyboard);

            DeserializeVersion(keyboardElement, keyboard);
            DeserializeLayers(keyboardElement, keyboard);
        }

        private static void DeserializeVersion(XElement keyboardElement, Keyboard keyboard)
        {
            if (XmlUtilities.TryGetAttribute(keyboardElement, "Version", out XAttribute versionAttribute))
            {
                keyboard.Version = versionAttribute.ValueAsVersion(keyboard);
            }
        }

        private static void DeserializeLayers(XElement keyboardElement, Keyboard keyboard)
        {
            if (XmlUtilities.TryGetSubElement(keyboardElement, "Layers", out XElement layersElement))
            {
                IEnumerable<XElement> layerElements = layersElement
                    .Nodes()
                    .Where(node =>
                        node.NodeType == System.Xml.XmlNodeType.Element
                        && ((XElement)node).Name == "Layer")
                    .Select(node => (XElement)node);

                IEnumerable<Layer> layers = layerElements
                    .Select(layerElement => DeserializeLayer(keyboard, layerElement))
                    .ToList();

                keyboard.Layers = layers;
            }
        }

        private static Layer DeserializeLayer(Keyboard parent, XElement layerElement)
        {
            var layer = new Layer { Parent = parent };
            LayerDeserializer.Default.Deserialize(layerElement, layer);
            return layer;
        }
    }
}
