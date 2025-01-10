using System.Text.Json;

namespace Guppy.Core.Resources.Common.ResourceTypes
{
    public interface IResourceType
    {
        Type Type { get; }
        string Name { get; }

        bool TryResolve(IResourcePack pack, string resource, string localization, JsonElement json);
    }
}