namespace KbUtil.Lib.Models.Keyboard
{
    using KbUtil.Lib.Models.Attributes;

    [GroupChild]
    public class Circle : Element
    {
        public float Size { get; set; }
        public override float Height => Size;
        public override float Width => Size;
    }
}
