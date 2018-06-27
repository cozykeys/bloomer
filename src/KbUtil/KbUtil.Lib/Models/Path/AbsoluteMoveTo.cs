namespace KbUtil.Lib.Models.Path
{
    using Geometry;

    public class AbsoluteMoveTo : IPathComponent
    {
        public Vec2 EndPoint { get; set; }

        public string Data => $"M {EndPoint.X} {EndPoint.Y}";
    }
}
