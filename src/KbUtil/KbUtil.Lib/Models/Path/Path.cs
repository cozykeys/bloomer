namespace KbUtil.Lib.Models.Path
{
    using KbUtil.Lib.Models.Attributes;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;
    using System.Linq;

    [GroupChild]
    public class Path : Element
    {
        public IEnumerable<IPathComponent> Components { get; set; }

        public string Data => string.Join(" ", Components.Select(component => component.Data));
    }
}
