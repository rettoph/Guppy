namespace Guppy.Core.Files.Common
{
    public struct FileLocation
    {
        public readonly DirectoryLocation Directory;
        public readonly string Name;
        public string Path => System.IO.Path.Combine(this.Directory.Path, this.Name);

        public FileLocation(DirectoryLocation directory, string name)
        {
            this.Directory = directory;
            this.Name = name;
        }

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
