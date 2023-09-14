using Guppy.Files.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files
{
    internal class GuppyFile
    {

    }
    internal class GuppyFile<T> : GuppyFile, IFile<T>
        where T : new()
    {
        public FileType Type { get; init; }

        public string Path { get; init; } = string.Empty;

        public string FullPath { get; init; } = string.Empty;

        public T Value { get; set; } = default!;
    }
}
