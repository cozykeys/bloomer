namespace KbUtil.Lib.Models.Keyboard
{
    using System;
    using System.Linq;

    public class Stack : Group
    {
        public StackOrientation Orientation { get; set; }

        public override float Width
        {
            get
            {
                switch (Orientation)
                {
                    case StackOrientation.Horizontal:
                        return Children.Sum(child => child.Width + child.Margin * 2);

                    case StackOrientation.Vertical:
                        if (Children == null || !Children.Any())
                        {
                            return default;
                        }

                        Func<Element, float> totalWidth = (element) => { return element.Width + element.Margin * 2; };
                        Func<Element, float> getMinX = (element) => { return (-totalWidth(element) / 2) + element.XOffset; };
                        Func<Element, float> getMaxX = (element) => { return (totalWidth(element) / 2) + element.XOffset; };

                        float minX = getMinX(Children.ElementAt(0));
                        float maxX = getMaxX(Children.ElementAt(0));

                        foreach(var child in Children.Skip(1))
                        {
                            minX = Math.Min(minX, getMinX(child));
                            maxX = Math.Max(maxX, getMaxX(child));
                        }

                        return maxX - minX;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override float Height
        {
            get
            {
                switch (Orientation)
                {
                    case StackOrientation.Horizontal:
                        if (Children == null || !Children.Any())
                        {
                            return default;
                        }

                        Func<Element, float> totalHeight = (element) => { return element.Height + element.Margin * 2; };
                        Func<Element, float> getMinY = (element) => { return (-totalHeight(element) / 2) + element.YOffset; };
                        Func<Element, float> getMaxY = (element) => { return (totalHeight(element) / 2) + element.YOffset; };

                        float minY = getMinY(Children.ElementAt(0));
                        float maxY = getMaxY(Children.ElementAt(0));

                        foreach(var child in Children.Skip(1))
                        {
                            minY = Math.Min(minY, getMinY(child));
                            maxY = Math.Max(maxY, getMaxY(child));
                        }

                        return maxY - minY;

                    case StackOrientation.Vertical:
                        return Children.Sum(child => child.Height + child.Margin * 2);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
