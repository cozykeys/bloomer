namespace KbUtil.Lib.Models.Keyboard
{
    public class Legend : Element
    {
        public string Text { get; set; }
        public float FontSize { get; set; }
        public LegendHorizontalAlignment HorizontalAlignment { get; set; }
        public LegendVerticalAlignment VerticalAlignment { get; set; }
        /// <summary>
        /// TODO: Strongly type this instead of using string
        /// </summary>
        public string Color { get; set; }
    }
}
