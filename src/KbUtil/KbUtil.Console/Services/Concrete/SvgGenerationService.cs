namespace KbUtil.Console.Services.Concrete
{
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.SvgGeneration;

    internal class SvgGenerationService : ISvgGenerationService
    {
        public void GenerateSvg(Keyboard keyboard, string path, SvgGenerationOptions options = null)
            => SvgGenerator.GenerateSvg(keyboard, path, options ?? new SvgGenerationOptions());
    }
}
