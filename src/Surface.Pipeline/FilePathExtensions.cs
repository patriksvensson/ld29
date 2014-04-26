using Lunt.IO;

namespace Surface.Pipeline
{
    internal static class FilePathExtensions
    {
        public static bool Exists(this FilePath path, IFileSystem fileSystem)
        {
            var file = fileSystem.GetFile(path);
            return file != null && file.Exists;
        }
    }
}