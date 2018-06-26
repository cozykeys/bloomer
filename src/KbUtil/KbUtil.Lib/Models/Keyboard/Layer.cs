namespace KbUtil.Lib.Models.Keyboard
{
    using System.Collections.Generic;

    public class Layer : Element
    {
        public int ZIndex { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}
