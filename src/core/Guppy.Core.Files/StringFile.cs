using Guppy.Core.Files.Common;

namespace Guppy.Core.Files
{
    internal class StringFile(FileLocation location, FileLocation source, string content) : IFile
    {
        public FileLocation Location { get; } = location;
        public FileLocation Source { get; } = source;

        public string Content { get; set; } = content;

        public bool Success { get; private set; }
    }
}
