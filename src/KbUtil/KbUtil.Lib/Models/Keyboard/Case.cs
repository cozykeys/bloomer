namespace KbUtil.Lib.Models.Keyboard
{
    using System.Collections.Generic;

    public class Case : Element
    {
        public Perimeter Perimeter { get; set; }
        public IEnumerable<Hole> Holes { get; set; }
    }
}
