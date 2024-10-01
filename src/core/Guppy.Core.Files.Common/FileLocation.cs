namespace Guppy.Core.Files.Common
{
    public struct FileLocation(DirectoryLocation directory, string name)
    {
        public readonly DirectoryLocation Directory = directory;
        public readonly string Name = name;
        public string Path => System.IO.Path.Combine(this.Directory.Path, this.Name);

        public static FileLocation AppData(string path, string name)
        {
            return new FileLocation(DirectoryLocation.AppData(path), name);
        }

        public static FileLocation CurrentDirectory(string path, string name)
        {
            return new FileLocation(DirectoryLocation.CurrentDirectory(path), name);
        }

        public static FileLocation Source(string path, string name)
        {
            return new FileLocation(DirectoryLocation.Source(path), name);
        }

        public override string ToString()
        {
            return $"{this.Directory}/{this.Name}";
        }
    }
}
