namespace Atreus87Generator.Commands
{
    using System;
    using Atreus87Generator.Services;
    using Microsoft.Extensions.CommandLineUtils;

    internal class GenerateSvgCommand
    {
        private readonly IFileService _fileService;

        private readonly CommandArgument _inputPathArgument;
        private readonly CommandArgument _outputPathArgument;

        public GenerateSvgCommand(IApplicationService applicationService, IFileService fileService)
        {
            _fileService = fileService;

            Command = applicationService.CommandLineApplication
                .Command("gen-svg", config =>
                {
                    config.Description = "Generate an SVG file from an XML input file.";
                    config.ExtendedHelpText = "TODO";
                    config.OnExecute(() => Execute());
                });

            _inputPathArgument = Command.Argument("<input-path>", "The path to the keyboard layout data file.");
            _outputPathArgument = Command.Argument("<output-path>", "The path to the generated SVG file.");
        }

        public CommandLineApplication Command { get; }

        public string InputPath => _inputPathArgument.Value;

        public string OutputPath => _outputPathArgument.Value;

        public int Execute()
        {
            ParseInputFile();
            return 0;
        }

        private void ParseInputFile()
        {
            try
            {
                _fileService.ReadAllText(InputPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read the keyboard data file at ${InputPath}.", ex);
            }
        }
    }
}
