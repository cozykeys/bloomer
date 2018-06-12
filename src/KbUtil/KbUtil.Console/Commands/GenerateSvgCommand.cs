namespace KbUtil.Console.Commands
{
    using Microsoft.Extensions.CommandLineUtils;

    using KbUtil.Console.Services;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.SvgGeneration;

    internal class GenerateSvgCommand
    {
        private readonly IKeyboardDataService _keyboardDataService;
        private readonly ISvgGenerationService _svgGenerationService;

        private readonly CommandArgument _inputPathArgument;
        private readonly CommandArgument _outputPathArgument;

        public GenerateSvgCommand(
            IApplicationService applicationService,
            IKeyboardDataService keyboardDataService,
            ISvgGenerationService svgGenerationService)
        {
            _keyboardDataService = keyboardDataService;
            _svgGenerationService = svgGenerationService;

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
            Keyboard keyboard = _keyboardDataService.GetKeyboardData(InputPath);

            var generationOptions = new SvgGenerationOptions();
            _svgGenerationService.GenerateSvg(keyboard, OutputPath, generationOptions);

            return 0;
        }
    }
}
