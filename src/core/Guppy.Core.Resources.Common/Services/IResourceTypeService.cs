using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Resources.Common.ResourceTypes;

namespace Guppy.Core.Resources.Common.Services
{
    public interface IResourceTypeService
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}