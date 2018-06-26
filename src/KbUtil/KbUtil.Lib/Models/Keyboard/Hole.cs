namespace KbUtil.Lib.Models.Keyboard
{
    using KbUtil.Lib.Models.Attributes;
    using System.Collections.Generic;

    [GroupChild]
    public class Hole : Element
    {
        public float Size { get; set; }
        public override float Height => Size;
        public override float Width => Size;
    }
}
