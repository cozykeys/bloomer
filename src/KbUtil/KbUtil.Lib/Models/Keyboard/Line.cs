namespace KbUtil.Lib.Models.Keyboard
{
    /// TODO: This should just be merged with <see cref="Side"/>
    internal class Line
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public float M => (P2.YOffset - P1.YOffset) / (P2.XOffset - P1.XOffset);
        public float B => P1.YOffset - M * P1.XOffset;
    }
}
