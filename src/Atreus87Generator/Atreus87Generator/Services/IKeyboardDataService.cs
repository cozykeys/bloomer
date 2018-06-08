namespace Atreus87Generator.Services
{
    using Atreus87Generator.Models;

    internal interface IKeyboardDataService
    {
        Keyboard GetKeyboardData(string path);
    }
}
