using Guppy.Core.Resources.ResourceTypes;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Resources.Services
{
    public interface IResourceTypeService
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}
