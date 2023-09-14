using Guppy.Attributes;
using Guppy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Resources
{
    [Service<IResourceTypeResolver>(ServiceLifetime.Singleton, true)]
    public interface IResourceTypeResolver
    {
        Type Type { get; }

        bool TryResolve(ResourcePack pack, Resource resource, string localization, string value);
    }
}
