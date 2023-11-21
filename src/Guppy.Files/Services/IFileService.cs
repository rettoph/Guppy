using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Services
{
    public interface IFileService
    {
        IFile Get(FileType type, string path, bool forceLoadFromDisk = false);
        IFile<T> Get<T>(FileType type, string path, bool forceLoadFromDisk = false)
            where T : new();

        void Save<T>(IFile<T> file)
            where T : new();
    }
}
