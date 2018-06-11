using System.Collections.Generic;

namespace KbUtil.Lib.Models.Keyboard
{
    public class Group : Element
    {
        public IEnumerable<Element> Children { get; set; }
    }
}
