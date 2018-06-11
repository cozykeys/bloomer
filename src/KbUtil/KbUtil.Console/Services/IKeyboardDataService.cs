namespace KbUtil.Console.Services
{
    using KbUtil.Lib.Models.Keyboard;

    internal interface IKeyboardDataService
    {
        Keyboard GetKeyboardData(string path);
    }
}
