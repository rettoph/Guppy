using System.Runtime.InteropServices;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Assets.Common.Extensions.System;

namespace Guppy.Core.Assets.Common
{
    public interface IAssetKey : IEquatable<IAssetKey>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        IAsset CreateAsset();
    }

    public readonly struct AssetKey<T> : IAssetKey, IEquatable<AssetKey<T>>
        where T : notnull
    {
        private readonly UnmanagedReference<IAssetKey, string> _name;
        private readonly UnmanagedReference<Setting<T>, T?> _default;

        public readonly Guid Id;
        public readonly string Name => this._name.Value;
        public readonly Type Type => typeof(T);
        public readonly T? DefaultValue => this._default.Value;

        readonly Guid IAssetKey.Id => this.Id;

        readonly string IAssetKey.Name => this.Name;

        private unsafe AssetKey(string name, T? defaultValue)
        {
            this._name = new UnmanagedReference<IAssetKey, string>(name);
            this._default = new UnmanagedReference<Setting<T>, T?>(defaultValue);

            this.Id = name.xxHash128();
        }

        public readonly void Dispose()
        {
            StaticCollection<IAssetKey>.Remove(this, false);

            _cache.Remove(this.Name);

            this._name.Dispose();
        }

        public override readonly bool Equals(object? obj)
        {
            return this.Equals((AssetKey<T>)obj!);
        }

        public readonly bool Equals(AssetKey<T> other)
        {
            return this.Id.Equals(other.Id);
        }

        public readonly bool Equals(IAssetKey? other)
        {
            return other is AssetKey<T> casted
                && this.Equals(casted);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public static bool operator ==(AssetKey<T> left, AssetKey<T> right)
        {
            return EqualityComparer<AssetKey<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetKey<T> left, AssetKey<T> right)
        {
            return !(left == right);
        }

        private static readonly Dictionary<string, AssetKey<T>> _cache = [];
        public static AssetKey<T> Get(string name, T? defaultValue = default)
        {
            ref AssetKey<T> resource = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out bool exists);
            if (exists)
            {
                return resource;
            }

            resource = new AssetKey<T>(name, defaultValue);
            StaticCollection<IAssetKey>.Add(resource);

            return resource;
        }

        public static IEnumerable<AssetKey<T>> GetAll()
        {
            return _cache.Values;
        }

        readonly IAsset IAssetKey.CreateAsset()
        {
            return new Asset<T>(this);
        }
    }
}