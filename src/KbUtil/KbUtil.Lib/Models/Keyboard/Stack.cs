namespace KbUtil.Lib.Models.Keyboard
{
    using System.Linq;

    public class Stack : Group
    {
        public StackOrientation Orientation { get; set; }

        public override float Width => Orientation == StackOrientation.Horizontal
            ? Children.Sum(child => child.Width + child.Margin * 2)
            : Children.Max(child => child.Width + child.Margin * 2);

        public override float Height => Orientation == StackOrientation.Horizontal
            ? Children.Max(child => child.Height + child.Margin * 2)
            : Children.Sum(child => child.Height + child.Margin * 2);
    }
}
