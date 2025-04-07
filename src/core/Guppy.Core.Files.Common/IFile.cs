namespace Guppy.Core.Files.Common
{
    public interface IFile
    {
        FilePath Path { get; }
        string Content { get; set; }
    }

    public interface IFile<T> : IFile
    {
        T Value { get; set; }
        bool Success { get; }
    }
}