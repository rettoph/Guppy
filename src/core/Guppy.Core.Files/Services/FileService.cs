using Guppy.Engine.Serialization;
using Guppy.Core.Files.Helpers;
using System.Runtime.InteropServices;

namespace Guppy.Core.Files.Services
{
    internal class FileService : IFileService
    {
        private readonly IJsonSerializer _json;
        private readonly IPathService _paths;
        private Dictionary<string, IFile> _cache;

        public FileService(IJsonSerializer json, IPathService paths)
        {
            _json = json;
            _paths = paths;
            _cache = new Dictionary<string, IFile>();
        }

        public IFile Get(FileLocation location, bool forceLoadFromDisk = false)
        {
            FileLocation source = _paths.GetSourceLocation(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, location.Path, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(source.Path);

                using (FileStream stream = File.Open(source.Path, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();

                        file = new StringFile(
                            location: location,
                            source: source,
                            content: content);
                    }
                }
            }

            return file ?? throw new Exception();
        }

        public IFile<T> Get<T>(FileLocation location, bool forceLoadFromDisk = false)
        {
            FileLocation source = _paths.GetSourceLocation(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, location.Path, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(source.Path);

                using (FileStream stream = File.Open(source.Path, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();

                        file = new JsonFile<T>(
                            location: location,
                            source: source,
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

        public void Save<T>(IFile<T> file)
        {
            DirectoryHelper.EnsureDirectoryExists(file.Source.Directory.Path);

            string json = _json.Serialize(file.Value);

            File.WriteAllText(file.Source.Path, json);
        }
    }
}
