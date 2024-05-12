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

        public bool TryResolve(ResourcePack pack, string resourceName, string localization, JsonElement json)
        {
            Resource<T> resource = Resource<T>.Get(resourceName);

            if (this.TryGetResolver(resource, pack.RootDirectory, ref json, out ResourceResolver<T>? resolver))
            {
                pack.Add(resource, localization, resolver);
                return true;
            }

            return false;
        }

        protected abstract bool TryGetResolver(Resource<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out ResourceResolver<T> resolver);
    }

    public abstract class SimpleResourceType<T> : ResourceType<T>
        where T : notnull
    {
        protected override bool TryGetResolver(Resource<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out ResourceResolver<T> resolver)
        {
            string input = json.GetString() ?? string.Empty;
            if (this.TryResolve(resource, root, input, out T value) == true)
            {
                resolver = new ResourceResolver<T>(() => value);
                return true;
            }

            resolver = null;
            return false;
        }

        protected abstract bool TryResolve(Resource<T> resource, DirectoryLocation root, string input, out T value);
    }

    public class DefaultResourceType<T> : ResourceType<T>
        where T : notnull
    {
        private readonly IJsonSerializationService _json;

        protected DefaultResourceType(IJsonSerializationService json)
        {
            _json = json;
        }

        protected override bool TryGetResolver(Resource<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out ResourceResolver<T> resolver)
        {
            T value = _json.Deserialize<T>(ref json, out bool success);
            resolver = new ResourceResolver<T>(() => value);

            return success;
        }
    }
}
