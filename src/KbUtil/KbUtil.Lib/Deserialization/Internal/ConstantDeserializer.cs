namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Deserialization.Extensions;
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    internal class ConstantDeserializer : IDeserializer<Constant>
    {
        public static ConstantDeserializer Default { get; set; } = new ConstantDeserializer();

        public void Deserialize(XElement constantElement, Constant constant)
        {
            ElementDeserializer.Default.Deserialize(constantElement, constant);

            DeserializeValue(constantElement, constant);
        }

        private void DeserializeValue(XElement constantElement, Constant constant)
        {
            if(XmlUtilities.TryGetAttribute(constantElement, "Value", out XAttribute valueAttribute))
            {
                constant.Value = valueAttribute.ValueAsString();
            }
        }
    }
}
