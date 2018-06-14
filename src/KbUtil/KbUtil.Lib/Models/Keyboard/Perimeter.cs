using System.Collections.Generic;

namespace KbUtil.Lib.Models.Keyboard
{
    public class Perimeter : Element
    {
        public IEnumerable<Side> Sides { get; set; }
        public IEnumerable<Corner> Corners { get; set; }
    }
}
