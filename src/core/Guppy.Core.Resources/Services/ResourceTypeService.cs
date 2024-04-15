﻿using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Resources.Common.Services;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Resources.Services
{
    internal sealed class ResourceTypeService : IResourceTypeService
    {
        private readonly Dictionary<string, IResourceType> _types;

        public ResourceTypeService(IEnumerable<IResourceType> types)
        {
            _types = types.ToDictionary(x => x.Name, x => x);
        }

        public bool TryGet(string name, [MaybeNullWhen(false)] out IResourceType resourceType)
        {
            return _types.TryGetValue(name, out resourceType);
        }
    }
}
