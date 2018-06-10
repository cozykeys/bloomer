using System.Collections.Generic;

namespace KbUtil.Lib.Models.Keyboard
{
    public class KbElementGroup : KbElement
    {
        public IEnumerable<KbElement> Children { get; set; }
    }
}
