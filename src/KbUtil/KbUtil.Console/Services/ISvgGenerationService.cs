namespace KbUtil.Console.Services
{
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.SvgGeneration;

    internal interface ISvgGenerationService
    {
        void GenerateSvg(Keyboard keyboard, string path, SvgGenerationOptions options = null);
    }
}
