namespace KbUtil.Lib.Models.Path
{
    using Geometry;

    public class AbsoluteLineTo : IPathComponent
    {
        public Vec2 EndPoint { get; set; }
    }
}
