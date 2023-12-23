using Guppy.Attributes;
using Guppy.Enums;
using System.Text.Json;

namespace Guppy.Resources.ResourceTypes
{
    [Service<IResourceType>(ServiceLifetime.Singleton, true)]
    public interface IResourceType
    {
        Type Type { get; }
        string Name { get; }

        bool TryResolve(ResourcePack pack, string resource, string localization, JsonElement json);
    }
}
