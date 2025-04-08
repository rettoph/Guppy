namespace Guppy.Core.Files.Common
{
    public readonly struct FilePath(DirectoryPath directoryPath, string fileName)
    {
        public readonly DirectoryPath DirectoryPath = directoryPath;
        public readonly string FileName = fileName;

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
            return $"{this.DirectoryPath}/{this.FileName}";
        }
    }
}