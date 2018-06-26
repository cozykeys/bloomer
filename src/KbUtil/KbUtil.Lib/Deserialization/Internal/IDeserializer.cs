namespace KbUtil.Lib.Deserialization.Internal
{
    using System.Xml.Linq;

    internal interface IDeserializer<T>
    {
        void Deserialize(XElement xElement, T item);
    }
}
