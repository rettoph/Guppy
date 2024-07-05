using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Common
{
    public interface IResourceValue : IRef, IDisposable
    {
        IResource Resource { get; }

        new object Value { get; }

        IEnumerable<object> All();

        void Clear();

        internal void Refresh(IResourcePackService resourcePackService);
    }

    public struct ResourceValue<T> : IResourceValue, IRef<T>
        where T : notnull
    {
        private readonly UnmanagedReference<ResourceValue<T>, List<T>> _value;

        public readonly Resource<T> Resource;

        public bool HasValue => _value.Value is not null;
        public T Value
        {
            get => _value.Value.First();
            set => _value.Value.Insert(0, value);
        }

        Type IRef.Type => this.Resource.Type;
        IResource IResourceValue.Resource => this.Resource;
        object IResourceValue.Value => this.Value;
        object? IRef.Value => this.Value;

        public ResourceValue(Resource<T> resource) : this(resource, Enumerable.Empty<T>())
        {
        }
        public ResourceValue(Resource<T> resource, IEnumerable<T> values) : this()
        {
            _value = new UnmanagedReference<ResourceValue<T>, List<T>>(values.ToList());

            this.Resource = resource;
        }

        public T GetValueOrFallback(T fallback)
        {
            if (_value.Value.Count > 0)
            {
                return _value.Value[0];
            }

            return fallback;
        }

        public IEnumerable<T> All()
        {
            return _value.Value;
        }

        public void Dispose()
        {
            _value.Dispose();
        }

        IEnumerable<object> IResourceValue.All()
        {
            return this.All().OfType<object>();
        }

        void IResourceValue.Clear()
        {
            _value.Value.Clear();
        }

        void IResourceValue.Refresh(IResourcePackService resourcePackService)
        {
            _value.Value.Clear();

            foreach (T value in resourcePackService.GetDefinedValues(this.Resource))
            {
                this.Value = value;
            }
        }

        public static implicit operator T(ResourceValue<T> resource)
        {
            return resource.Value;
        }
    }
}
