using Guppy.Resources.ResourceTypes;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Resources.Providers
{
    public interface IResourceTypeProvider
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType);
    }
}
