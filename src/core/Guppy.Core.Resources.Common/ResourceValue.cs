﻿using Guppy.Core.Common;
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

    public readonly struct ResourceValue<T> : IResourceValue, IRef<T>
        where T : notnull
    {
        private readonly UnmanagedReference<ResourceValue<T>, List<T>> _value;

        public readonly Resource<T> Resource;

        public readonly bool HasValue => _value.Value is not null;
        public readonly T Value
        {
            get => _value.Value.First();
            set => _value.Value.Insert(0, value);
        }

        readonly Type IRef.Type => this.Resource.Type;

        readonly IResource IResourceValue.Resource => this.Resource;

        readonly object IResourceValue.Value => this.Value;

        readonly object? IRef.Value => this.Value;

        public ResourceValue(Resource<T> resource) : this(resource, resource.DefaultValue)
        {
        }
        public ResourceValue(Resource<T> resource, T? value) : this(resource, value is null ? [] : value.Yield())
        {
        }
        public ResourceValue(Resource<T> resource, IEnumerable<T> values) : this()
        {
            _value = new UnmanagedReference<ResourceValue<T>, List<T>>(values.ToList());

            this.Resource = resource;
        }

        public readonly IEnumerable<T> All()
        {
            return _value.Value;
        }

        public readonly void Dispose()
        {
            _value.Dispose();
        }

        readonly IEnumerable<object> IResourceValue.All()
        {
            return this.All().OfType<object>();
        }

        readonly void IResourceValue.Clear()
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
