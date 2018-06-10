namespace KbUtil.Console.Services.Concrete
{
    using System;

    internal class EnvironmentService : IEnvironmentService
    {
        public string NewLine() => Environment.NewLine;
    }
}
