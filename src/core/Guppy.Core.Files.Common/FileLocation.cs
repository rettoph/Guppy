namespace Guppy.Core.Files.Common
{
    public readonly struct FileLocation(DirectoryLocation directory, string name)
    {
        public readonly DirectoryLocation Directory = directory;
        public readonly string Name = name;
        public readonly string Path => System.IO.Path.Combine(this.Directory.Path, this.Name);

        public static FileLocation AppData(string path, string name)
        {
            return new(DirectoryLocation.AppData(path), name);
        }

        public static FileLocation AppData(string name)
        {
            return new(DirectoryLocation.AppData(), name);
        }

        public static FileLocation CurrentDirectory(string path, string name)
        {
            return new(DirectoryLocation.CurrentDirectory(path), name);
        }

        public static FileLocation CurrentDirectory(string name)
        {
            return new(DirectoryLocation.CurrentDirectory(), name);
        }

        public static FileLocation Source(string path, string name)
        {
            return new(DirectoryLocation.Source(path), name);
        }

        public static FileLocation Source(string name)
        {
            return new(DirectoryLocation.Source(), name);
        }

        public override readonly string ToString()
        {
            return $"{this.Directory}/{this.Name}";
        }
    }
}