namespace KbUtil.Lib.Models.Keyboard
{
    public class Element
    {
        public string Name { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float Rotation { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public float Margin { get; set; }
        public Element Parent { get; set; }
    }
}
