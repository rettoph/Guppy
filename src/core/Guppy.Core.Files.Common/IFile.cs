namespace Guppy.Core.Files.Common
{
    public interface IFile
    {
        FilePath Location { get; }
        FilePath Source { get; }
        string Content { get; set; }
    }

    public interface IFile<T> : IFile
    {
        T Value { get; set; }
        bool Success { get; }
    }
}