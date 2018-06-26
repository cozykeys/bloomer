namespace KbUtil.Lib.Models.Keyboard
{
    using KbUtil.Lib.Models.Attributes;
    using System;
    using System.Linq;

    [GroupChild]
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
                        return Children.Sum(child => ((Element)child).Width + ((Element)child).Margin * 2);

                    case StackOrientation.Vertical:
                        if (Children == null || !Children.Any())
                        {
                            return default;
                        }

                        Func<Element, float> totalWidth = (element) => { return element.Width + element.Margin * 2; };
                        Func<Element, float> getMinX = (element) => { return (-totalWidth(element) / 2) + element.XOffset; };
                        Func<Element, float> getMaxX = (element) => { return (totalWidth(element) / 2) + element.XOffset; };

                        float minX = getMinX((Element)Children.ElementAt(0));
                        float maxX = getMaxX((Element)Children.ElementAt(0));

                        foreach(var child in Children.Skip(1))
                        {
                            minX = Math.Min(minX, getMinX((Element)child));
                            maxX = Math.Max(maxX, getMaxX((Element)child));
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

                        float minY = getMinY((Element)Children.ElementAt(0));
                        float maxY = getMaxY((Element)Children.ElementAt(0));

                        foreach(var child in Children.Skip(1))
                        {
                            minY = Math.Min(minY, getMinY((Element)child));
                            maxY = Math.Max(maxY, getMaxY((Element)child));
                        }

                        return maxY - minY;

                    case StackOrientation.Vertical:
                        return Children.Sum(child => ((Element)child).Height + ((Element)child).Margin * 2);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
