using System.Collections.Generic;

namespace KbUtil.Lib.Models.Keyboard
{
    public class Perimeter : Element
    {
        public IEnumerable<Edge> Edges { get; set; }
    }
}
