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
        public required FileType Type { get; init; }

        public required string Path { get; init; }

        public required string FullPath { get; init; }
        public required bool Success { get; init; }

        public T Value { get; set; } = default!;
    }
}
