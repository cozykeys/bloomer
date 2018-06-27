namespace KbUtil.Lib.Models.Path
{
    using KbUtil.Lib.Models.Geometry;

    public class RelativeQuadraticCurveTo : IPathComponent
    {
        public Vec2 EndPoint { get; set; }
        public Vec2 ControlPoint { get; set; }

        public string Data => throw new System.NotImplementedException();
    }
}
