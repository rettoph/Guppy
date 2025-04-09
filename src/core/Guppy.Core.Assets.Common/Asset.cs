using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Core.Assets.Common
{
    public interface IAsset : IRef, IDisposable
    {
        IAssetKey Key { get; }

        new object? Value { get; }

        IEnumerable<object> All();

        void Clear();

        void Refresh(IAssetPackService resourcePackService);
    }

    public readonly struct Asset<T> : IAsset, IRef<T?>
        where T : notnull
    {
        private readonly UnmanagedReference<Asset<T>, List<T>> _value;

        public readonly AssetKey<T> Key;

        public readonly bool HasValue => this._value.Value is not null && this._value.Value.Count > 0;
        public readonly T? Value
        {
            get => this._value.Value.FirstOrDefault();
            set => this._value.Value.Insert(0, value ?? throw new NotImplementedException());
        }

        readonly Type IRef.Type => this.Key.Type;

        readonly IAssetKey IAsset.Key => this.Key;

        readonly object? IAsset.Value => this.Value;

        readonly object? IRef.Value => this.Value;

        public Asset(AssetKey<T> key) : this(key, key.DefaultValue)
        {
        }
        public Asset(AssetKey<T> key, T? value) : this(key, value is null ? [] : value.Yield())
        {
        }
        public Asset(AssetKey<T> key, IEnumerable<T> values) : this()
        {
            this._value = new UnmanagedReference<Asset<T>, List<T>>(values.ToList());

            this.Key = key;
        }

        public readonly IEnumerable<T> All()
        {
            return this._value.Value;
        }

        public readonly void Dispose()
        {
            this._value.Dispose();
        }

        readonly IEnumerable<object> IAsset.All()
        {
            return this.All().OfType<object>();
        }

        readonly void IAsset.Clear()
        {
            this._value.Value.Clear();
        }

        void IAsset.Refresh(IAssetPackService resourcePackService)
        {
            this._value.Value.Clear();

            foreach (T value in resourcePackService.GetDefinedValues(this.Key))
            {
                if (value is null)
                {
                    continue;
                }

                this.Value = value;
            }
        }

        public static implicit operator T(Asset<T> resource)
        {
            return resource.Value ?? throw new NotImplementedException();
        }
    }
}