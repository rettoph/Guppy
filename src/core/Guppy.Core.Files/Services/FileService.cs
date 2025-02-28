﻿using System.Runtime.InteropServices;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Files.Services
{
    internal class FileService(IJsonSerializationService json, IPathService paths) : IFileService
    {
        private readonly IJsonSerializationService _json = json;
        private readonly IPathService _paths = paths;
        private readonly Dictionary<string, IFile> _cache = [];

        public IFile Get(FileLocation location, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false)
        {
            FileLocation source = this._paths.GetSourceLocation(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(this._cache, location.Path, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(source.Path);

                using FileStream stream = File.Open(source.Path, createIfDoesNotExist ? FileMode.OpenOrCreate : FileMode.Open);
                using StreamReader reader = new(stream);
                string content = reader.ReadToEnd();

                file = new StringFile(
                    location: location,
                    source: source,
                    content: content);
            }

            return file ?? throw new Exception();
        }

        public IFile<T> Get<T>(FileLocation location, bool forceLoadFromDisk = false, bool createIfDoesNotExist = false)
        {
            FileLocation source = this._paths.GetSourceLocation(location);
            ref IFile? file = ref CollectionsMarshal.GetValueRefOrAddDefault(this._cache, location.Path, out bool exists);

            if (!exists || forceLoadFromDisk)
            {
                DirectoryHelper.EnsureDirectoryExists(source.Path);

                using FileStream stream = File.Open(source.Path, createIfDoesNotExist ? FileMode.OpenOrCreate : FileMode.Open);
                using StreamReader reader = new(stream);
                string content = reader.ReadToEnd();

                file = new JsonFile<T>(
                    location: location,
                    source: source,
                    content: content,
                    json: this._json);
            }
            else if (file is StringFile stringFile)
            {
                file = new JsonFile<T>(stringFile, this._json);
            }

            return file as IFile<T> ?? throw new Exception();
        }

        public void Save<T>(IFile<T> file)
        {
            DirectoryHelper.EnsureDirectoryExists(file.Source.Directory.Path);

            string json = this._json.Serialize(file.Value);

            File.WriteAllText(file.Source.Path, json);
        }
    }
}