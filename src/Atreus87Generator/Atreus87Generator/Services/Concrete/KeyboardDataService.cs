namespace Atreus87Generator.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Atreus87Generator.Models;

    internal class KeyboardDataService : IKeyboardDataService
    {
        public Keyboard GetKeyboardData(string path)
        {
            XElement keyboardData = LoadInputData(path);
            return DeserializeKeyboard(keyboardData);
        }

        private static XElement LoadInputData(string path)
        {
            try
            {
                return XElement.Load(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load the XML input file at ${path}.", ex);
            }
        }
        private static Keyboard DeserializeKeyboard(XElement keyboardData)
        {
            if (keyboardData.Name != "Keyboard")
            {
                throw new Exception("TODO");
            }

            return new Keyboard
            {
                KeyboardRegions = DeserializeRegions(keyboardData.Nodes())
            };
        }

        private static IEnumerable<KeyboardRegion> DeserializeRegions(IEnumerable<XNode> regions)
            => regions.Select(DeserializeRegion);

        private static KeyboardRegion DeserializeRegion(XNode region)
        {
            return new KeyboardRegion
            {
                Components = DeserializeComponents(((XElement)region).Nodes())
            };
        }

        private static IEnumerable<IKeyboardRegionComponent> DeserializeComponents(IEnumerable<XNode> components)
            => components.Select(DeserializeComponent);

        private static IKeyboardRegionComponent DeserializeComponent(XNode component)
        {
            return new KeyColumn();
        }
    }
}
