using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Helpers
{
    public static class DirectoryHelper
    {
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
