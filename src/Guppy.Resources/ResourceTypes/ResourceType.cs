using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Guppy.Resources.ResourceTypes
{
    public abstract class ResourceType<T> : IResourceType
        where T : notnull
    {
        public Type Type => typeof(T);

        public virtual string Name => this.Type.Name;

        public bool TryResolve(ResourcePack pack, string resourceName, string localization, ref Utf8JsonReader reader)
        {
            Resource<T> resource = Resource.Get<T>(resourceName);

            if (this.TryResolve(resource, pack.RootDirectory, ref reader, out T? value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected abstract bool TryResolve(Resource<T> resource, string root, ref Utf8JsonReader reader, [MaybeNullWhen(false)] out T value);
    }

    public abstract class SimpleResourceType<T> : ResourceType<T>
        where T : notnull
    {
        protected override bool TryResolve(Resource<T> resource, string root, ref Utf8JsonReader reader, out T value)
        {
            reader.CheckToken(JsonTokenType.String, true);
            string input = reader.GetString() ?? string.Empty;

            return this.TryResolve(resource, root, input, out value);
        }

        protected abstract bool TryResolve(Resource<T> resource, string root, string input, out T value);
    }
}
