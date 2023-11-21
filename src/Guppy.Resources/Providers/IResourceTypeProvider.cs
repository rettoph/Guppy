using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Resources.ResourceTypes;

namespace Guppy.Resources.Providers
{
    public interface IResourceTypeProvider
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}
