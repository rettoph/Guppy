namespace Guppy.Files
{
    public interface IFile
    {
        FileLocation Location { get; }
        FileLocation Source { get; }
        string Content { get; set; }
    }
    public interface IFile<T> : IFile
    {
        T Value { get; set; }
        bool Success { get; }
    }
}
