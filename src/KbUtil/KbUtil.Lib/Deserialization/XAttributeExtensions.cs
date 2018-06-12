namespace KbUtil.Lib.Deserialization
{
    using System;
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Keyboard;

    public static class XAttributeExtensions
    {
        public static string ValueAsString(this XAttribute attribute) => attribute.Value;
        public static int ValueAsInt(this XAttribute attribute) => int.Parse(attribute.Value);
        public static float ValueAsFloat(this XAttribute attribute) => float.Parse(attribute.Value);
        public static bool ValueAsBool(this XAttribute attribute) => bool.Parse(attribute.Value);
        public static Version ValueAsVersion(this XAttribute attribute) => Version.Parse(attribute.Value);
        public static LegendHorizontalAlignment ValueAsLegendHorizontalAlignment(this XAttribute attribute)
            => (LegendHorizontalAlignment)Enum
                .Parse(typeof(LegendHorizontalAlignment), attribute.ValueAsString());
        public static LegendVerticalAlignment ValueAsLegendVerticalAlignment(this XAttribute attribute)
            => (LegendVerticalAlignment)Enum
                .Parse(typeof(LegendVerticalAlignment), attribute.ValueAsString());
        public static StackOrientation ValueAsStackOrientation(this XAttribute attribute)
            => (StackOrientation)Enum.Parse(typeof(StackOrientation), attribute.ValueAsString());
    }
}
