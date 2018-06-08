namespace Atreus87Generator.Services
{
    using Microsoft.Extensions.CommandLineUtils;

    internal interface IApplicationService
    {
        CommandLineApplication CommandLineApplication { get; }
    }
}
