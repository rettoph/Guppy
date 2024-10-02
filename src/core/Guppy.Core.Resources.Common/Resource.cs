using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Extensions.System;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Common
{
    public interface IResource : IEquatable<IResource>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        internal IResourceValue CreateValue();
    }

    public readonly struct Resource<T> : IResource, IEquatable<Resource<T>>
        where T : notnull
    {
        private readonly UnmanagedReference<IResource, string> _name;
        private readonly UnmanagedReference<Setting<T>, T?> _default;

        public readonly Guid Id;
        public readonly string Name => _name.Value;
        public readonly Type Type => typeof(T);
        public readonly T? DefaultValue => _default.Value;

        readonly Guid IResource.Id => this.Id;

        readonly string IResource.Name => this.Name;

        private unsafe Resource(string name, T? defaultValue)
        {
            _name = new UnmanagedReference<IResource, string>(name);
            _default = new UnmanagedReference<Setting<T>, T?>(defaultValue);

            this.Id = name.xxHash128();
        }

        public readonly void Dispose()
        {
            StaticCollection<IResource>.Remove(this, false);

            _cache.Remove(this.Name);

            _name.Dispose();
        }

        public override readonly bool Equals(object? obj)
        {
            return Equals((Resource<T>)obj!);
        }

        public readonly bool Equals(Resource<T> other)
        {
            return Id.Equals(other.Id);

        }

        public readonly bool Equals(IResource? other)
        {
            return other is Resource<T> casted
                && this.Equals(casted);
        }

        public override readonly int GetHashCode()
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

        private static readonly Dictionary<string, Resource<T>> _cache = [];
        public static Resource<T> Get(string name, T? defaultValue = default)
        {
            ref Resource<T> resource = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out bool exists);
            if (exists)
            {
                return resource;
            }

            resource = new Resource<T>(name, defaultValue);
            StaticCollection<IResource>.Add(resource);

            return resource;
        }

        public static IEnumerable<Resource<T>> GetAll()
        {
            return _cache.Values;
        }

        readonly IResourceValue IResource.CreateValue()
        {
            return new ResourceValue<T>(this);
        }
    }
}
