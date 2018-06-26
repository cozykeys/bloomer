namespace KbUtil.Lib.Models.Path
{
    using Geometry;

    public class AbsoluteCubicCurveTo : IPathComponent
    {
        public Vec2 End { get; set; }
        public Vec2 ControlA { get; set; }
        public Vec2 ControlB { get; set; }
    }
}