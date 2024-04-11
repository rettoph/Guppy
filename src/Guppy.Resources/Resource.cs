using Guppy.Common;
using Guppy.Common.Utilities;
using Guppy.Resources.Services;
using Guppy.Resources.Utilities;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public interface IResource : IEquatable<IResource>, IDisposable
    {
        Guid Id { get; }
        UnmanagedString Name { get; }
        Type Type { get; }

        internal void Initialize(ResourceService resources);
    }

    public struct Resource<T> : IResource, IEquatable<Resource<T>>, IRef<T>
        where T : notnull
    {
        private readonly int _index;

        public readonly Guid Id;
        public readonly UnmanagedString Name;
        public Type Type => typeof(T);
        public T Value
        {
            get => StaticValueCollection<Resource<T>, T>.Get(_index);
            set => StaticValueCollection<Resource<T>, T>.Set(_index, value);
        }

        Guid IResource.Id => this.Id;
        UnmanagedString IResource.Name => this.Name;

        private unsafe Resource(string name)
        {
            _index = StaticValueCollection<Resource<T>, T>.Pop();

            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = new UnmanagedString(name);
        }

        public void Dispose()
        {
            _cache.Remove(this.Name);

            StaticValueCollection<Resource<T>, T>.Push(_index);
            StaticCollection<IResource>.Remove(this, false);

            this.Name.Dispose();
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

        public static void Clear()
        {
            while (_cache.Count > 0)
            {
                _cache.Values.First().Dispose();
            }

            _cache.Clear();

            StaticValueCollection<Resource<T>, T>.Clear();
        }
    }
}
