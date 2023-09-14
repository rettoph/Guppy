using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Providers
{
    internal class DefaultFilePathProvider : IFileTypePathProvider
    {
        public const string GAME_NAME = "Guppy";
        public string GetFullPath(FileType type, string path)
        {
            return type switch
            {
                FileType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GAME_NAME, path),
                FileType.CurrentDirectory => Path.Combine(Directory.GetCurrentDirectory(), path),
                _ => path
            };
        }
    }
}
