using System;
using System.Xml.Linq;

namespace KbUtil.Console.Deserializers
{
    public static class XAttributeExtensions
    {
        public static string StringValue(this XAttribute attribute) => attribute.Value;
        public static int IntValue(this XAttribute attribute) => int.Parse(attribute.Value);
        public static float FloatValue(this XAttribute attribute) => float.Parse(attribute.Value);
        public static Version VersionValue(this XAttribute attribute) => Version.Parse(attribute.Value);
    }
}
