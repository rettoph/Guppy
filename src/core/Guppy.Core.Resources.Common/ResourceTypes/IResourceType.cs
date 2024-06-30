using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using System.Text.Json;

namespace Guppy.Core.Resources.Common.ResourceTypes
{
    [Service<IResourceType>(ServiceLifetime.Singleton, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public interface IResourceType
    {
        Type Type { get; }
        string Name { get; }

        bool TryResolve(ResourcePack pack, string resource, string localization, JsonElement json);
    }
}
