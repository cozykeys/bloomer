namespace KbUtil.Lib.Models.Path
{
    using Geometry;

    public class RelativeLineTo : IPathComponent
    {
        public Vec2 EndPoint { get; set; }

        public string Data => throw new System.NotImplementedException();
    }
}
