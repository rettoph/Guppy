using Guppy.Core.Files.Common;

namespace Guppy.Core.Files
{
    internal class StringFile(FilePath location, FilePath source, string content) : IFile
    {
        public FilePath Location { get; } = location;
        public FilePath Source { get; } = source;

        public string Content { get; set; } = content;

        public bool Success { get; private set; }
    }
}