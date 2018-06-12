namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class LegendWriter : IElementWriter<Legend>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Legend keyboard)
        {
            throw new System.NotImplementedException();
        }

        public void WriteAttributes(XmlWriter writer, Legend keyboard)
        {
            throw new System.NotImplementedException();
        }

        public void WriteSubElements(XmlWriter writer, Legend keyboard)
        {
            throw new System.NotImplementedException();
        }
    }
}
