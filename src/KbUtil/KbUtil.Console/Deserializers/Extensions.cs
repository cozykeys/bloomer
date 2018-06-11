using KbUtil.Lib.Models.Keyboard;
using System;
using System.Xml.Linq;

namespace KbUtil.Console.Deserializers
{
    public static class XAttributeExtensions
    {
        public static string ValueAsString(this XAttribute attribute) => attribute.Value;
        public static int ValueAsInt(this XAttribute attribute) => int.Parse(attribute.Value);
        public static float ValueAsFloat(this XAttribute attribute) => float.Parse(attribute.Value);
        public static Version ValueAsVersion(this XAttribute attribute) => Version.Parse(attribute.Value);
        public static LegendHorizontalAlignment ValueAsLegendHorizontalAlignment(this XAttribute attribute)
            => (LegendHorizontalAlignment)Enum
                .Parse(typeof(LegendHorizontalAlignment), attribute.ValueAsString());
        public static LegendVerticalAlignment ValueAsLegendVerticalAlignment(this XAttribute attribute)
            => (LegendVerticalAlignment)Enum
                .Parse(typeof(LegendVerticalAlignment), attribute.ValueAsString());
    }
}
