using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Assets.Common.AssetTypes;

namespace Guppy.Core.Assets.Common.Services
{
    public interface IAssetTypeService
    {
        bool TryGet(string name, [MaybeNullWhen(false)] out IAssetType resourceType);
    }
}