using Guppy.Files.Enums;

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
    {
        T Value { get; set; }
        bool Success { get; }
    }
}
