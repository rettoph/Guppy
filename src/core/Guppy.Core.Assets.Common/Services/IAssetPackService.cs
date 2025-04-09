namespace Guppy.Core.Assets.Common.Services
{
    public interface IAssetPackService
    {
        IEnumerable<IAssetPack> GetAll();
        IAssetPack GetById(Guid id);

        IEnumerable<IAssetKey> GetDefinedAssets();
        IEnumerable<T> GetDefinedValues<T>(AssetKey<T> resource) where T : notnull;
    }
}