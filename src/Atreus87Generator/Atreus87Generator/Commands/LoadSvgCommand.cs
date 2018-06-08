namespace Atreus87Generator.Commands
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using Atreus87Generator.Services;
    using Microsoft.Extensions.CommandLineUtils;

    internal class LoadSvgCommand
    {
        private readonly CommandArgument _inputPathArgument;

        public LoadSvgCommand(IApplicationService applicationService)
        {
            Command = applicationService.CommandLineApplication
                .Command("load-svg", config =>
                {
                    config.Description = "TODO";
                    config.ExtendedHelpText = "TODO";
                    config.OnExecute(() => Execute());
                });

            _inputPathArgument = Command.Argument("<input-path>", "The path to the keyboard layout data file.");
        }

        public CommandLineApplication Command { get; }

        public string InputPath => _inputPathArgument.Value;

        public int Execute()
        {
            XElement svgRootElement = LoadSvg(InputPath);

            foreach (XNode node in svgRootElement.Nodes())
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                var regex = new Regex(@"\{.*\}(.*)", RegexOptions.IgnoreCase);
                Match m = regex.Match(((XElement)node).Name.ToString());

                if (!m.Success || m.Groups[1].Value != "g")
                {
                    continue;
                }

                HandleGroupElement((XElement)node);
            }


            return 0;
        }

        private static void HandleGroupElement(XElement element)
        {
        }

        private static XElement LoadSvg(string path)
        {
            try
            {
                return XElement.Load(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load the SVG input file at ${path}.", ex);
            }
        }

        private void HandleElement(XElement element)
        {
        }
    }
}
