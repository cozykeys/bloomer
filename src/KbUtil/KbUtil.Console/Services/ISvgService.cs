using KbUtil.Lib.Models.Keyboard;

namespace KbUtil.Console.Services
{
    internal interface ISvgService
    {
        void GenerateSvg(KbElementKeyboard kbElementKeyboard, string path);
    }
}
