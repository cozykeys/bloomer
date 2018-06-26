namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class LayerDeserializer : IDeserializer<Layer>
    {
        public static LayerDeserializer Default { get; set; } = new LayerDeserializer();

        public void Deserialize(XElement layerElement, Layer layer)
        {
            ElementDeserializer.Default.Deserialize(layerElement, layer);

            DeserializeZIndex(layerElement, layer);
            DeserializeGroups(layerElement, layer);
        }

        private void DeserializeZIndex(XElement layerElement, Layer layer)
        {
            if (XmlUtilities.TryGetAttribute(layerElement, "ZIndex", out XAttribute zIndexAttribute))
            {
                layer.ZIndex = zIndexAttribute.ValueAsInt(layer);
            }
        }

        private void DeserializeGroups(XElement layerElement, Layer layer)
        {
            if (XmlUtilities.TryGetSubElement(layerElement, "Groups", out XElement groupsElement))
            {
                IEnumerable<XElement> groupElements = groupsElement
                    .Nodes()
                    .Where(node =>
                        node.NodeType == System.Xml.XmlNodeType.Element
                        && ((XElement)node).Name == "Group")
                    .Select(node => (XElement)node);

                IEnumerable<Group> groups = groupElements
                    .Select(groupElement => DeserializeGroup(layer, groupElement))
                    .ToList();

                layer.Groups = groups;
            }
        }

        private static Group DeserializeGroup(Layer parent, XElement groupElement)
        {
            var group = new Group { Parent = parent };
            GroupDeserializer.Default.Deserialize(groupElement, group);
            return group;
        }
    }
}
