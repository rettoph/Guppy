using System.Text.Json;

namespace Guppy.Core.Assets.Common.AssetTypes
{
    public interface IAssetType
    {
        Type Type { get; }
        string Name { get; }

        bool TryResolve(IAssetPack pack, string resource, string localization, JsonElement json);
    }
}