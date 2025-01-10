using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Files.Common;

namespace Guppy.Core.Resources.Common
{
    public interface IResourcePack
    {
        Guid Id { get; }
        string Name { get; }
        DirectoryLocation RootDirectory { get; }

        public void Add<T>(ResourceKey<T> key, string localization, T value)
            where T : notnull;

        public void Add<T>(ResourceKey<T> key, T value)
            where T : notnull;

        public bool TryGetDefinedValue<T>(ResourceKey<T> key, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull;

        public IEnumerable<ResourceKey<T>> GetAllDefinedResources<T>()
            where T : notnull;

        public IEnumerable<IResourceKey> GetAllDefinedResources();
    }
}