using Guppy.Core.Files.Common;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Resources.Common
{
    public interface IResourcePack
    {
        Guid Id { get; }
        string Name { get; }
        DirectoryLocation RootDirectory { get; }

        public void Add<T>(Resource<T> resource, string localization, T value)
            where T : notnull;

        public void Add<T>(Resource<T> resource, T value)
            where T : notnull;

        public bool TryGetDefinedValue<T>(Resource<T> resource, string localization, [MaybeNullWhen(false)] out T value)
            where T : notnull;

        public IEnumerable<Resource<T>> GetAllDefinedResources<T>()
            where T : notnull;

        public IEnumerable<IResource> GetAllDefinedResources();
    }
}
