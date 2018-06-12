namespace KbUtil.Lib.Models.Keyboard
{
    public abstract class Element
    {
        public string Name { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float Rotation { get; set; }
        public virtual float Height { get; set; }
        public virtual float Width { get; set; }
        public float Margin { get; set; }
        public Element Parent { get; set; }
        public bool Debug { get; set; }
    }
}
