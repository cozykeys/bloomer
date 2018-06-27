namespace KbUtil.Lib.Models.Path
{
    using KbUtil.Lib.Models.Geometry;

    public class AbsoluteQuadraticCurveTo : IPathComponent
    {
        public Vec2 EndPoint { get; set; }
        public Vec2 ControlPoint { get; set; }

        public string Data => $"Q {ControlPoint.X} {ControlPoint.Y} {EndPoint.X} {EndPoint.Y}";
    }
}
