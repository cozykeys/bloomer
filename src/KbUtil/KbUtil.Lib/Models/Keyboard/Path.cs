namespace KbUtil.Lib.Models.Keyboard
{
    using KbUtil.Lib.Models.Attributes;
    using System.Collections.Generic;

    [GroupChild]
    public class Path : Element
    {
        public IEnumerable<Side> Sides { get; set; }
        public IEnumerable<Corner> Corners { get; set; }
    }
}
