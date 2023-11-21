using Guppy.Files.Enums;
using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files
{
    internal class StringFile : IFile
    {
        public FileType Type { get; }

        public string Path { get; }

        public string FullPath { get; }

        public string Content { get; set; }

        public bool Success { get; private set; }

        public StringFile(FileType type, string path, string fullPath, string content)
        {
            this.Type = type;
            this.Path = path;
            this.FullPath = fullPath;
            this.Content = content;
        }
    }
}
