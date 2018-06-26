namespace KbUtil.Lib.Deserialization.Extensions
{
    using System;
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Keyboard;

    public static class XAttributeExtensions
    {
        public static string ValueAsString(this XAttribute attribute)
            => attribute.Value;
        public static string ValueAsString(this XAttribute attribute, Element element)
            => attribute.Value.ReplaceConstants(element);

        public static int ValueAsInt(this XAttribute attribute)
            => attribute.Value.ToInt();
        public static int ValueAsInt(this XAttribute attribute, Element element)
            => attribute.Value.ReplaceConstants(element).ToInt();

        public static float ValueAsFloat(this XAttribute attribute)
            => attribute.Value.ToFloat();
        public static float ValueAsFloat(this XAttribute attribute, Element element)
            => attribute.Value.ReplaceConstants(element).ToFloat();

        public static bool ValueAsBool(this XAttribute attribute)
            => attribute.Value.ToBool();
        public static bool ValueAsBool(this XAttribute attribute, Element element)
            => attribute.Value.ReplaceConstants(element).ToBool();

        public static Version ValueAsVersion(this XAttribute attribute)
            => attribute.Value.ToVersion();
        public static Version ValueAsVersion(this XAttribute attribute, Element element)
            => attribute.Value.ReplaceConstants(element).ToVersion();

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
