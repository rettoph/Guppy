using Guppy.Files.Enums;
using Guppy.Files.Providers;
using Guppy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Services
{
    internal class FileService : IFileService
    {
        private readonly IJsonSerializer _json;
        private readonly IFileTypePathProvider _paths;
        private Dictionary<string, GuppyFile> _cache;

        public FileService(IJsonSerializer json, IFileTypePathProvider paths)
        {
            _json = json;
            _paths = paths;
            _cache = new Dictionary<string, GuppyFile>();
        }

        public IFile<T> Get<T>(FileType type, string path, bool forceLoadFromDisk = false)
            where T : new()
        {
            string fullPath = _paths.GetFullPath(type, path);

            ref GuppyFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, fullPath, out bool exists);

            if(!exists || forceLoadFromDisk)
            {
                this.EnsureDirectoryExists(fullPath);

                using (FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();
                        T value = _json.Deserialize<T>(content) ?? new T();

                        file = new GuppyFile<T>()
                        {
                            Type = type,
                            Path = path,
                            FullPath = fullPath,
                            Value = value
                        };
                    }
                }
            }

            return file as IFile<T> ?? throw new Exception();
        }

        public void Save<T>(IFile<T> file)
            where T : new()

        {
            this.EnsureDirectoryExists(file.FullPath);

            string json = _json.Serialize(file.Value);

            File.WriteAllText(file.FullPath, json);
        }

        private void EnsureDirectoryExists(string fullPath)
        {
            string directory = Path.GetDirectoryName(fullPath) ?? throw new NotImplementedException();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
