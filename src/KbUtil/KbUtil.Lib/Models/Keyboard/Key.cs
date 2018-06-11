namespace KbUtil.Lib.Models.Keyboard
{
    using System.Collections.Generic;

    public class Key : Element
    {
        public IEnumerable<Legend> Legends { get; set; }
    }
}
