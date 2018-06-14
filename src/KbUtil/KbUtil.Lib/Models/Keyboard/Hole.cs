using System.Collections.Generic;

namespace KbUtil.Lib.Models.Keyboard
{
    public class Hole : Element
    {
        public static Dictionary<string, float> Sizes { get; } = new Dictionary<string, float>
        {
            { "M2Screw", 2.3f },
            { "M2Spacer", 3.563f },
        };

        public float Size { get; set; }
        public override float Height => Size;
        public override float Width => Size;
    }
}
