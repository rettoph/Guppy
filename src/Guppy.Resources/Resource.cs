using Guppy.Common;
using Guppy.Common.Utilities;
using Guppy.Resources.Providers;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public interface IResource : IEquatable<IResource>, IDisposable
    {
        Guid Id { get; }
        UnmanagedString Name { get; }
        Type Type { get; }

        internal void Initialize(ResourceProvider resources);
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
            get => _values[_index];
            set => _values[_index] = value;
        }

        Guid IResource.Id => this.Id;
        UnmanagedString IResource.Name => this.Name;

        internal unsafe Resource(string name)
        {
            _index = Resource<T>.PopValueIndex();

            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = new UnmanagedString(name);

            ResourceHelper.Add(this);
        }

        public void Dispose()
        {
            _cache.Remove(this.Name);

            ResourceHelper.Remove(this);

            Resource<T>.PushValueIndex(_index);

            this.Name.Dispose();
        }

        void IResource.Initialize(ResourceProvider resources)
        {
            this.SetValue(resources);
        }

        internal void SetValue(ResourceProvider resources)
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
            _values.Clear();
            _indices.Clear();
        }

        private static Stack<int> _indices = new Stack<int>();
        private static List<T> _values = new List<T>();
        private static int PopValueIndex()
        {
            if (_indices.TryPop(out int index))
            {
                return index;
            }

            _values.Add(default!);
            return _values.Count - 1;
        }
        private static void PushValueIndex(int index)
        {
            if (_values[index] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _indices.Push(index);
        }
    }

    internal static class ResourceHelper
    {
        private static List<IResource> _values = new List<IResource>();

        public static event EventHandler<IResource>? OnAdded;

        public static IEnumerable<IResource> GetAll()
        {
            return _values;
        }

        public static void Add(IResource value)
        {
            _values.Add(value);

            ResourceHelper.OnAdded?.Invoke(null, value);
        }

        public static void Remove(IResource value)
        {
            _values.Remove(value);
        }

        internal static void Clear()
        {
            // TODO: This should clear and dispose all resources
            // throw new NotImplementedException();
        }
    }
}
