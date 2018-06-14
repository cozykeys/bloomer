namespace KbUtil.Lib.Models.Keyboard
{
    public class Edge : Element
    {
        public enum Style
        {
            Straight,
            Curved
        }

        public Point A { get; set; }
        public Point B { get; set; }
    }
}
