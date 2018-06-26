namespace KbUtil.Lib.Deserialization
{
    using KbUtil.Lib.Deserialization.Internal;
    using KbUtil.Lib.Models.Keyboard;
    using System.Xml.Linq;

    public class KeyboardDataDeserializer
    {
        public static Keyboard DeserializeKeyboard(XElement rootElement)
        {
            var keyboard = new Keyboard();
            KeyboardDeserializer.Default.Deserialize(rootElement, keyboard);
            return keyboard;
        }
    }
}
