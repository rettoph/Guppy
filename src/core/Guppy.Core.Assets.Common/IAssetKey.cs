namespace Guppy.Core.Assets.Common
{
    public interface IAssetKey : IEquatable<IAssetKey>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        IAsset CreateAsset();
    }
}
