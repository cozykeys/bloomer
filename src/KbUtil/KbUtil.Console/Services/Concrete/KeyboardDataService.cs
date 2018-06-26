namespace KbUtil.Console.Services.Concrete
{
    using System;
    using System.Xml.Linq;
    using KbUtil.Lib.Models.Keyboard;
    using KbUtil.Lib.Deserialization;

    internal class KeyboardDataService : IKeyboardDataService
    {
        public Keyboard GetKeyboardData(string path)
        {
            XElement rootElement = LoadInputData(path);

            if (rootElement.Name != "Keyboard")
            {
                throw new Exception("The root element of the input file should be of type Keyboard.");
            }

            return KeyboardDataDeserializer.DeserializeKeyboard(rootElement);
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
    }
}
