using Guppy.Attributes;
using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
