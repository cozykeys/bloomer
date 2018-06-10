namespace KbUtil.Console.Services
{
    using Microsoft.Extensions.CommandLineUtils;

    internal interface IApplicationService
    {
        CommandLineApplication CommandLineApplication { get; }
    }
}
