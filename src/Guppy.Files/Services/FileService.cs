using Guppy.Files.Enums;
using Guppy.Files.Helpers;
using Guppy.Files.Providers;
using Guppy.Serialization;
using System.Runtime.InteropServices;

namespace Guppy.Files.Services
{
    internal class FileService : IFileService
    {
        private readonly IJsonSerializer _json;
        private readonly IFilePathProvider _paths;
        private Dictionary<string, IFile> _cache;

        public FileService(IJsonSerializer json, IFilePathProvider paths)
        {
            _json = json;
            _paths = paths;
            _cache = new Dictionary<string, IFile>();
        }

        public IFile Get(FileLocation location, bool forceLoadFromDisk = false)
        {
            string fullPath = _paths.GetFullPath(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, fullPath, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(fullPath);

                using (FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();

                        file = new StringFile(
                            location: location,
                            fullPath: fullPath,
                            content: content);
                    }
                }
            }

            return file ?? throw new Exception();
        }

        public IFile Get(FileType type, string path, bool forceLoadFromDisk = false)
        {
            return this.Get(new FileLocation(type, path), forceLoadFromDisk);
        }

        public IFile<T> Get<T>(FileLocation location, bool forceLoadFromDisk = false)
        {
            string fullPath = _paths.GetFullPath(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, fullPath, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(fullPath);

                using (FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();

                        file = new JsonFile<T>(
                            location: location,
                            fullPath: fullPath,
                            content: content,
                            json: _json);
                    }
                }
            }
            else if (file is StringFile stringFile)
            {
                file = new JsonFile<T>(stringFile, _json);
            }

            return file as IFile<T> ?? throw new Exception();
        }

        public IFile<T> Get<T>(FileType type, string path, bool forceLoadFromDisk = false)
        {
            return this.Get<T>(new FileLocation(type, path), forceLoadFromDisk);
        }

        public void Save<T>(IFile<T> file)
        {
            DirectoryHelper.EnsureDirectoryExists(file.FullPath);

            string json = _json.Serialize(file.Value);

            File.WriteAllText(file.FullPath, json);
        }
    }
}
