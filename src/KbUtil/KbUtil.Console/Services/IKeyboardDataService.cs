namespace KbUtil.Console.Services
{
    using KbUtil.Lib.Models.Keyboard;

    internal interface IKeyboardDataService
    {
        KbElementKeyboard GetKeyboardData(string path);
    }
}
