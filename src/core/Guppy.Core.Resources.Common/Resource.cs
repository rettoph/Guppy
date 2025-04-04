using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Common
{
    public interface IResource : IRef, IDisposable
    {
        IResourceKey Key { get; }

        new object? Value { get; }

        IEnumerable<object> All();

        void Clear();

        void Refresh(IResourcePackService resourcePackService);
    }

    public readonly struct Resource<T> : IResource, IRef<T?>
        where T : notnull
    {
        private readonly UnmanagedReference<Resource<T>, List<T>> _value;

        public readonly ResourceKey<T> Key;

        public readonly bool HasValue => this._value.Value is not null && this._value.Value.Count > 0;
        public readonly T? Value
        {
            get => this._value.Value.FirstOrDefault();
            set => this._value.Value.Insert(0, value ?? throw new NotImplementedException());
        }

        readonly Type IRef.Type => this.Key.Type;

        readonly IResourceKey IResource.Key => this.Key;

        readonly object? IResource.Value => this.Value;

        readonly object? IRef.Value => this.Value;

        public Resource(ResourceKey<T> key) : this(key, key.DefaultValue)
        {
        }
        public Resource(ResourceKey<T> key, T? value) : this(key, value is null ? [] : value.Yield())
        {
        }
        public Resource(ResourceKey<T> key, IEnumerable<T> values) : this()
        {
            this._value = new UnmanagedReference<Resource<T>, List<T>>(values.ToList());

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

        readonly IEnumerable<object> IResource.All()
        {
            return this.All().OfType<object>();
        }

        readonly void IResource.Clear()
        {
            this._value.Value.Clear();
        }

        void IResource.Refresh(IResourcePackService resourcePackService)
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

        public static implicit operator T(Resource<T> resource)
        {
            return resource.Value ?? throw new NotImplementedException();
        }
    }
}