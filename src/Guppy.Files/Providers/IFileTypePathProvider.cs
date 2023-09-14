using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Providers
{
    internal interface IFileTypePathProvider
    {
        string GetFullPath(FileType type, string path);
    }
}
