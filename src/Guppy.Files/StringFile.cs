namespace Guppy.Files
{
    internal class StringFile : IFile
    {
        public FileLocation Location { get; }

        public string FullPath { get; }

        public string Content { get; set; }

        public bool Success { get; private set; }

        public StringFile(FileLocation location, string fullPath, string content)
        {
            this.Location = location;
            this.FullPath = fullPath;
            this.Content = content;
        }
    }
}
