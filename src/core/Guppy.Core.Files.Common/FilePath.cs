namespace Guppy.Core.Files.Common
{
    public readonly struct FilePath(DirectoryPath directory, string fileName)
    {
        public readonly DirectoryPath Directory = directory;
        public readonly string FileName = fileName;
        public readonly string FullPath => Path.Combine(this.Directory.Path, this.FileName);

        public static FilePath AppData(string path, string name)
        {
            return new(DirectoryPath.AppData(path), name);
        }

        public static FilePath AppData(string name)
        {
            return new(DirectoryPath.AppData(), name);
        }

        public static FilePath CurrentDirectory(string path, string name)
        {
            return new(DirectoryPath.CurrentDirectory(path), name);
        }

        public static FilePath CurrentDirectory(string name)
        {
            return new(DirectoryPath.CurrentDirectory(), name);
        }

        public static FilePath Source(string path, string name)
        {
            return new(DirectoryPath.Source(path), name);
        }

        public static FilePath Source(string name)
        {
            return new(DirectoryPath.Source(), name);
        }

        public override readonly string ToString()
        {
            return $"{this.Directory}/{this.FileName}";
        }
    }
}