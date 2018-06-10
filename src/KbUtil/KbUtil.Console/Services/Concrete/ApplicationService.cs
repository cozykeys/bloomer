namespace KbUtil.Console.Services.Concrete
{
    using Microsoft.Extensions.CommandLineUtils;

    internal class ApplicationService : IApplicationService
    {
        public CommandLineApplication CommandLineApplication { get; }

        public ApplicationService()
        {
            CommandLineApplication = new CommandLineApplication
            {
                Description = "Lorem Ipsum",
                Name = "Lorem Ipsum",
                FullName = "Lorem Ipsum",
                ExtendedHelpText = "Lorem Ipsum"
            };
        }
    }
}
