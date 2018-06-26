namespace KbUtil.Lib.Models.Path
{
    using KbUtil.Lib.Models.Attributes;
    using KbUtil.Lib.Models.Keyboard;
    using System.Collections.Generic;

    [GroupChild]
    public class Path : Element
    {
        public IEnumerable<IPathComponent> Components { get; set; }
    }
}
