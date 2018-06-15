namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;

    internal interface IElementWriter<in T>
    {
        SvgGenerationOptions GenerationOptions { get; set; }
        void Write(XmlWriter writer, T element);
        void WriteAttributes(XmlWriter writer, T element);
        void WriteSubElements(XmlWriter writer, T element);
    }
}
