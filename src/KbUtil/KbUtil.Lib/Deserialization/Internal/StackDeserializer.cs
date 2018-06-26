namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class StackDeserializer : IDeserializer<Stack>
    {
        public static StackDeserializer Default { get; set; } = new StackDeserializer();

        public void Deserialize(XElement stackElement, Stack stack)
        {
            GroupDeserializer.Default.Deserialize(stackElement, stack);

            DeserializeOrientation(stackElement, stack);
        }

        private void DeserializeOrientation(XElement stackElement, Stack stack)
        {
            if(XmlUtilities.TryGetAttribute(stackElement, "Orientation", out XAttribute orientationAttribute))
            {
                stack.Orientation = orientationAttribute.ValueAsStackOrientation();
            }
        }
    }
}
