using Guppy.Core.Resources.Common.ResourceTypes;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourceTypeService
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}
