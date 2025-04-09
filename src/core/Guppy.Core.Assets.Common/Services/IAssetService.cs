namespace Guppy.Core.Assets.Common.Services
{
    public interface IAssetService
    {
        IEnumerable<AssetKey<T>> GetKeys<T>()
            where T : notnull;

        Asset<T> Get<T>(AssetKey<T> resource)
            where T : notnull;

        IEnumerable<Asset<T>> GetAll<T>()
            where T : notnull;
    }
}