namespace KbUtil.Lib.Models.Keyboard
{
    using System.Collections.Generic;

    public class Key : Element
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public IEnumerable<Legend> Legends { get; set; }
    }
}
