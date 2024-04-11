using Guppy.Common;
using Guppy.Common.Utilities;
using Guppy.Resources.Extensions.System;
using Guppy.Resources.Services;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public interface IResource : IEquatable<IResource>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        internal void Initialize(ResourceService resources);
    }

    public struct Resource<T> : IResource, IEquatable<Resource<T>>, IRef<T>
        where T : notnull
    {
        private readonly StaticValue<IResource, string> _name;
        private readonly StaticValue<IResource, T> _value;

        public readonly Guid Id;
        public readonly string Name => _name.Value;
        public Type Type => typeof(T);
        public T Value
        {
            get => _value.Value;
            set => _value.SetValue(value);
        }

        Guid IResource.Id => this.Id;
        string IResource.Name => this.Name;

        private unsafe Resource(string name)
        {
            _name = new StaticValue<IResource, string>(name);
            _value = new StaticValue<IResource, T>();

            this.Id = name.xxHash128();
        }

        public void Dispose()
        {
            StaticCollection<IResource>.Remove(this, false);

            _cache.Remove(this.Name);

            _name.Dispose();
            _value.Dispose();
        }

        void IResource.Initialize(ResourceService resources)
        {
            this.SetValue(resources);
        }

        internal void SetValue(ResourceService resources)
        {
            this.Value = resources.GetPackValue(this);
        }

        public override bool Equals(object? obj)
        {
            return Equals((Resource<T>)obj!);
        }

        public bool Equals(Resource<T> other)
        {
            return Id.Equals(other.Id);

        }

        public bool Equals(IResource? other)
        {
            return other is Resource<T> casted
                && this.Equals(casted);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Resource<T> left, Resource<T> right)
        {
            return EqualityComparer<Resource<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(Resource<T> left, Resource<T> right)
        {
            return !(left == right);
        }

        public static implicit operator T(Resource<T> resource)
        {
            return resource.Value;
        }

        private static readonly Dictionary<string, Resource<T>> _cache = new Dictionary<string, Resource<T>>();
        public static Resource<T> Get(string name)
        {
            ref Resource<T> resource = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out bool exists);
            if (exists)
            {
                return resource;
            }

            resource = new Resource<T>(name);
            StaticCollection<IResource>.Add(resource);

            return resource;
        }

        public static IEnumerable<Resource<T>> GetAll()
        {
            return _cache.Values;
        }
    }
}
