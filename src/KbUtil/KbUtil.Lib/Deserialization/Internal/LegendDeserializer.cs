namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Keyboard;

    internal class LegendDeserializer : IDeserializer<Legend>
    {
        public static LegendDeserializer Default { get; set; } = new LegendDeserializer();

        public void Deserialize(XElement legendElement, Legend legend)
        {
            ElementDeserializer.Default.Deserialize(legendElement, legend);

            DeserializeHorizontalAlignment(legendElement, legend);
            DeserializeVerticalAlignment(legendElement, legend);
            DeserializeText(legendElement, legend);
            DeserializeFontSize(legendElement, legend);
            DeserializeColor(legendElement, legend);
        }

        private void DeserializeHorizontalAlignment(XElement legendElement, Legend legend)
        {
            if(XmlUtilities.TryGetAttribute(legendElement, "HorizontalAlignment", out XAttribute horizontalAlignmentAttribute))
            {
                legend.HorizontalAlignment = horizontalAlignmentAttribute.ValueAsLegendHorizontalAlignment();
            }
        }

        private void DeserializeVerticalAlignment(XElement legendElement, Legend legend)
        {
            if(XmlUtilities.TryGetAttribute(legendElement, "VerticalAlignment", out XAttribute verticalAlignmentAttribute))
            {
                legend.VerticalAlignment = verticalAlignmentAttribute.ValueAsLegendVerticalAlignment();
            }
        }

        private void DeserializeText(XElement legendElement, Legend legend)
        {
            if(XmlUtilities.TryGetAttribute(legendElement, "Text", out XAttribute textAttribute))
            {
                legend.Text = textAttribute.ValueAsString(legend);
            }
        }

        private void DeserializeFontSize(XElement legendElement, Legend legend)
        {
            if(XmlUtilities.TryGetAttribute(legendElement, "FontSize", out XAttribute fontSizeAttribute))
            {
                legend.FontSize = fontSizeAttribute.ValueAsFloat(legend);
            }
        }

        private void DeserializeColor(XElement legendElement, Legend legend)
        {
            if(XmlUtilities.TryGetAttribute(legendElement, "Color", out XAttribute colorAttribute))
            {
                legend.Color = colorAttribute.ValueAsString(legend);
            }
        }
    }
}
