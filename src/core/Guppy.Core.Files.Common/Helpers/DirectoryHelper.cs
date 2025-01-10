using System.Reflection;
using Guppy.Core.Files.Common.Enums;

namespace Guppy.Core.Files.Common.Helpers
{
    public static class DirectoryHelper
    {
        public static void EnsureDirectoryExists(FileLocation source) => DirectoryHelper.EnsureDirectoryExists(source.Directory);

        public static void EnsureDirectoryExists(DirectoryLocation source)
        {
            if (source.Type != DirectoryTypeEnum.Source)
            {
                throw new ArgumentException($"Invalid {nameof(DirectoryLocation)} {nameof(DirectoryLocation.Type)}, {source.Type}", nameof(source));
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

        public static string Normalize(string path) => path.Replace("\\", "/");

        public static string Combine(params string[] paths) => DirectoryHelper.Normalize(Path.Combine(paths));

        public static string GetEntryDirectory() => DirectoryHelper.Normalize(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? Directory.GetCurrentDirectory());
    }
}