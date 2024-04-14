using Guppy.Resources.ResourceTypes;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Resources.Services
{
    public interface IResourceTypeService
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}
