using Guppy.Core.Files.Common;

namespace Guppy.Core.Files
{
    internal class StringFile(FilePath path, string content) : IFile
    {
        public FilePath Path { get; } = path;

        public string Content { get; set; } = content;

        public bool Success { get; private set; }
    }
}