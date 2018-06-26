namespace KbUtil.Lib.Models.Keyboard
{
    using System;
    using System.Collections.Generic;

    public class Keyboard : Element
    {
        public Version Version { get; set; }
        public IEnumerable<Layer> Layers { get; set; }
    }
}
