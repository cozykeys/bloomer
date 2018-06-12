namespace KbUtil.Console
{
    using System;
    using Microsoft.Extensions.CommandLineUtils;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using NLog;
    using NLog.Config;
    using NLog.Extensions.Logging;
    using NLog.Targets;

    using KbUtil.Console.Commands;
    using KbUtil.Console.Services;
    using KbUtil.Console.Services.Concrete;

    public static class Program
    {
        public static int Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddServices()
                .BuildServiceProvider();

            CreateCommands(serviceProvider);

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger(nameof(Program));

            try
            {
                CommandLineApplication commandLineApplication = serviceProvider
                    .GetService<IApplicationService>().CommandLineApplication;

                return commandLineApplication.Execute(args);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Unhandled {ex}");
                return -1;
            }
        }

        private static LoggingConfiguration GetNLogConfig()
        {
            var fileTarget = new FileTarget("logfile")
            {
                FileNameKind = FilePathKind.Absolute,
                FileName = $"${{specialfolder:folder=CommonApplicationData}}/{nameof(KbUtil.Console)}/Logs/application.log",
                ArchiveFileName = $"${{specialfolder:folder=CommonApplicationData}}/{nameof(KbUtil.Console)}/Logs/application.#.log",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
                ConcurrentWrites = true,
                KeepFileOpen = true,
                MaxArchiveFiles = 14,
                Layout = "${longdate}|${logger} ${ndc}|${uppercase:${level}}|${message}"
            };

            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = "${processtime:format=mm\\:ss.fff} | ${level:uppercase=true} | ${message}"
            };

            var config = new LoggingConfiguration();
            config.AddTarget(consoleTarget);
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(consoleTarget);
            config.AddRuleForAllLevels(fileTarget);

            return config;
        }

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            LogManager.Configuration = GetNLogConfig();
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddDebug()
                .AddNLog(new NLogProviderOptions { EventIdSeparator = "|" });

            serviceCollection.AddSingleton(loggerFactory);
            serviceCollection.AddSingleton<IAssemblyService, AssemblyService>();
            serviceCollection.AddSingleton<IEnvironmentService, EnvironmentService>();
            serviceCollection.AddSingleton<IApplicationService, ApplicationService>();
            serviceCollection.AddSingleton<IKeyboardDataService, KeyboardDataService>();
            serviceCollection.AddSingleton<IFileService, FileService>();
            serviceCollection.AddSingleton<ISvgGenerationService, SvgGenerationService>();
            return serviceCollection;
        }

        private static void CreateCommands(IServiceProvider serviceProvider)
        {
            ActivatorUtilities.CreateInstance<GenerateSvgCommand>(serviceProvider);
        }
    }
}
