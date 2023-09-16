using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files
{
    public interface IFile<T>
        where T : new()
    {
        FileType Type { get; }
        string Path { get; }
        string FullPath { get; }
        bool Success { get; }
        T Value { get; set; }
    }
}
