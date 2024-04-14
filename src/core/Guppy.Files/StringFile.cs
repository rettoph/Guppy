namespace Guppy.Files
{
    internal class StringFile : IFile
    {
        public FileLocation Location { get; }
        public FileLocation Source { get; }

        public string Content { get; set; }

        public bool Success { get; private set; }

        public StringFile(FileLocation location, FileLocation source, string content)
        {
            this.Location = location;
            this.Source = source;
            this.Content = content;
        }
    }
}
