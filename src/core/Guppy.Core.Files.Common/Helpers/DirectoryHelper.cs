using Guppy.Core.Files.Common.Enums;
using System.Reflection;

namespace Guppy.Core.Files.Common.Helpers
{
    public static class DirectoryHelper
    {
        public static void EnsureDirectoryExists(FileLocation source)
        {
            DirectoryHelper.EnsureDirectoryExists(source.Directory);
        }

        public static void EnsureDirectoryExists(DirectoryLocation source)
        {
            if (source.Type != DirectoryType.Source)
            {
                throw new ArgumentException();
            }

            if (!Directory.Exists(source.Path))
            {
                Directory.CreateDirectory(source.Path);
            }
        }

        public static void EnsureDirectoryExists(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath) ?? throw new NotImplementedException();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static string Normalize(string path)
        {
            return path.Replace("\\", "/");
        }

        public static string Combine(params string[] paths)
        {
            return DirectoryHelper.Normalize(Path.Combine(paths));
        }

        public static string GetEntryDirectory()
        {
            return DirectoryHelper.Normalize(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? Directory.GetCurrentDirectory());
        }
    }
}
