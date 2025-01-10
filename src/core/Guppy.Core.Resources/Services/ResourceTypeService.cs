using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Services
{
    internal sealed class ResourceTypeService(IEnumerable<IResourceType> types) : IResourceTypeService
    {
        private readonly Dictionary<string, IResourceType> _types = types.ToDictionary(x => x.Name, x => x);

        public bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType)
        {
            return this._types.TryGetValue(name, out resourceType);
        }
    }
}