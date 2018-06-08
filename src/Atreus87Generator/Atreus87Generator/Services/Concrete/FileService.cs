namespace Atreus87Generator.Services.Concrete
{
    using System.IO;

    internal class FileService : IFileService
    {
        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}
