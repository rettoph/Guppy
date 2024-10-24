using Guppy.Core.Files.Common;
using Guppy.Core.Serialization.Common.Services;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Guppy.Core.Resources.Common.ResourceTypes
{
    public abstract class ResourceType<T> : IResourceType
        where T : notnull
    {
        public Type Type => typeof(T);

        public virtual string Name => this.Type.Name;

        public bool TryResolve(IResourcePack pack, string resourceName, string localization, JsonElement json)
        {
            ResourceKey<T> resource = ResourceKey<T>.Get(resourceName);

            if (this.TryResolve(resource, pack.RootDirectory, ref json, out T? value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected abstract bool TryResolve(ResourceKey<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out T resolver);
    }

    public abstract class SimpleResourceType<T> : ResourceType<T>
        where T : notnull
    {
        protected override bool TryResolve(ResourceKey<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out T value)
        {
            string input = json.GetString() ?? string.Empty;
            return this.TryResolve(resource, root, input, out value);
        }

        protected abstract bool TryResolve(ResourceKey<T> resource, DirectoryLocation root, string input, out T value);
    }

    public class DefaultResourceType<T> : ResourceType<T>
        where T : notnull
    {
        private readonly IJsonSerializationService _json;

        protected DefaultResourceType(IJsonSerializationService json)
        {
            _json = json;
        }

        protected override bool TryResolve(ResourceKey<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out T value)
        {
            value = _json.Deserialize<T>(ref json, out bool success);

            return success;
        }
    }
}
