namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;

    internal interface IElementWriter<in T>
    {
        void Write(XmlWriter writer, T keyboard);
        void WriteAttributes(XmlWriter writer, T keyboard);
        void WriteSubElements(XmlWriter writer, T keyboard);
    }
}
