using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Extensions.System;
using Guppy.Core.Resources.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Common
{
    public interface IResource : IEquatable<IResource>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        internal void Refresh();
    }

    public struct Resource<T> : IResource, IEquatable<Resource<T>>, IRef<T>
        where T : notnull
    {
        private readonly UnmanagedReference<IResource, string> _name;
        private readonly UnmanagedReference<IResource, T> _value;

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
        object? IRef.Value => this.Value;

        private unsafe Resource(string name)
        {
            _name = new UnmanagedReference<IResource, string>(name);
            _value = new UnmanagedReference<IResource, T>();

            this.Id = name.xxHash128();
        }

        public void Dispose()
        {
            StaticCollection<IResource>.Remove(this, false);

            _cache.Remove(this.Name);

            _name.Dispose();
            _value.Dispose();
        }

        void IResource.Refresh()
        {
            this.Refresh();
        }

        internal void Refresh()
        {
            this.Value = Singleton<IResourceService>.Instance.GetLatestValue(this);
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
