namespace KbUtil.Console.Commands
{
    using KbUtil.Console.Services;
    using KbUtil.Lib.Models.Keyboard;
    using Microsoft.Extensions.CommandLineUtils;

    internal class GenerateSvgCommand
    {
        private readonly IKeyboardDataService _keyboardDataService;
        private readonly ISvgService _svgService;

        private readonly CommandArgument _inputPathArgument;
        private readonly CommandArgument _outputPathArgument;

        public GenerateSvgCommand(
            IApplicationService applicationService,
            IKeyboardDataService keyboardDataService,
            ISvgService svgService)
        {
            _keyboardDataService = keyboardDataService;
            _svgService = svgService;

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
            KbElementKeyboard keyboard = _keyboardDataService.GetKeyboardData(InputPath);

            _svgService.GenerateSvg(keyboard, OutputPath);

            return 0;
        }
    }
}
