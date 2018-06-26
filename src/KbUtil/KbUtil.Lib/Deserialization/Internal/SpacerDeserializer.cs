namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class SpacerDeserializer : IDeserializer<Spacer>
    {
        public static SpacerDeserializer Default { get; set; } = new SpacerDeserializer();

        public void Deserialize(XElement spacerElement, Spacer spacer)
        {
            ElementDeserializer.Default.Deserialize(spacerElement, spacer);
        }
    }
}
