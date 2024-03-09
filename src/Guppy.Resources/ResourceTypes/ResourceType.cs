using Guppy.Files;
using Guppy.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Guppy.Resources.ResourceTypes
{
    public abstract class ResourceType<T> : IResourceType
        where T : notnull
    {
        public Type Type => typeof(T);

        public virtual string Name => this.Type.Name;

        public bool TryResolve(ResourcePack pack, string resourceName, string localization, JsonElement json)
        {
            Resource<T> resource = Resource.Get<T>(resourceName);

            if (this.TryResolve(resource, pack.RootDirectory, ref json, out T? value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected abstract bool TryResolve(Resource<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out T value);
    }

    public abstract class SimpleResourceType<T> : ResourceType<T>
        where T : notnull
    {
        protected override bool TryResolve(Resource<T> resource, DirectoryLocation root, ref JsonElement json, out T value)
        {
            string input = json.GetString() ?? string.Empty;

            return this.TryResolve(resource, root, input, out value);
        }

        protected abstract bool TryResolve(Resource<T> resource, DirectoryLocation root, string input, out T value);
    }

    public class DefaultResourceType<T> : ResourceType<T>
        where T : notnull
    {
        private readonly IJsonSerializer _json;

        protected DefaultResourceType(IJsonSerializer json)
        {
            _json = json;
        }

        protected override bool TryResolve(Resource<T> resource, DirectoryLocation root, ref JsonElement json, [MaybeNullWhen(false)] out T value)
        {
            value = _json.Deserialize<T>(ref json, out bool success);

            return success;
        }
    }
}
