using Guppy.Common;
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
        private readonly IGuppyEnvironment _environment;

        public DefaultFilePathProvider(IGuppyEnvironment environment)
        {
            _environment = environment;
        }

        public string GetFullPath(FileType type, string path)
        {
            return type switch
            {
                FileType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _environment.Company, _environment.Name, path),
                FileType.CurrentDirectory => Path.Combine(Directory.GetCurrentDirectory(), path),
                _ => path
            };
        }
    }
}
