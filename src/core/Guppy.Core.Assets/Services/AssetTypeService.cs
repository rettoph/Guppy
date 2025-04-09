using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Core.Assets.Services
{
    internal sealed class AssetTypeService(IEnumerable<IAssetType> types) : IAssetTypeService
    {
        private readonly Dictionary<string, IAssetType> _types = types.ToDictionary(x => x.Name, x => x);

        public bool TryGet(string name, [MaybeNullWhen(false)] out IAssetType resourceType)
        {
            return this._types.TryGetValue(name, out resourceType);
        }
    }
}