using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files
{
    public interface IFile
    {
        FileType Type { get; }
        string Path { get; }
        string FullPath { get; }
        string Content { get; set; }
    }
    public interface IFile<T> : IFile
        where T : new()
    {
        T Value { get; set; }
        bool Success { get; }
    }
}
