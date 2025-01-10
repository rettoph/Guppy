using System.Runtime.InteropServices;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Extensions.System;

namespace Guppy.Core.Resources.Common
{
    public interface IResourceKey : IEquatable<IResourceKey>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        Type Type { get; }

        IResource CreateResource();
    }

    public readonly struct ResourceKey<T> : IResourceKey, IEquatable<ResourceKey<T>>
        where T : notnull
    {
        private readonly UnmanagedReference<IResourceKey, string> _name;
        private readonly UnmanagedReference<Setting<T>, T?> _default;

        public readonly Guid Id;
        public readonly string Name => this._name.Value;
        public readonly Type Type => typeof(T);
        public readonly T? DefaultValue => this._default.Value;

        readonly Guid IResourceKey.Id => this.Id;

        readonly string IResourceKey.Name => this.Name;

        private unsafe ResourceKey(string name, T? defaultValue)
        {
            this._name = new UnmanagedReference<IResourceKey, string>(name);
            this._default = new UnmanagedReference<Setting<T>, T?>(defaultValue);

            this.Id = name.xxHash128();
        }

        public readonly void Dispose()
        {
            StaticCollection<IResourceKey>.Remove(this, false);

            _cache.Remove(this.Name);

            this._name.Dispose();
        }

        public override readonly bool Equals(object? obj)
        {
            return this.Equals((ResourceKey<T>)obj!);
        }

        public readonly bool Equals(ResourceKey<T> other)
        {
            return this.Id.Equals(other.Id);

        }

        public readonly bool Equals(IResourceKey? other)
        {
            return other is ResourceKey<T> casted
                && this.Equals(casted);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public static bool operator ==(ResourceKey<T> left, ResourceKey<T> right)
        {
            return EqualityComparer<ResourceKey<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(ResourceKey<T> left, ResourceKey<T> right)
        {
            return !(left == right);
        }

        private static readonly Dictionary<string, ResourceKey<T>> _cache = [];
        public static ResourceKey<T> Get(string name, T? defaultValue = default)
        {
            ref ResourceKey<T> resource = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out bool exists);
            if (exists)
            {
                return resource;
            }

            resource = new ResourceKey<T>(name, defaultValue);
            StaticCollection<IResourceKey>.Add(resource);

            return resource;
        }

        public static IEnumerable<ResourceKey<T>> GetAll()
        {
            return _cache.Values;
        }

        readonly IResource IResourceKey.CreateResource()
        {
            return new Resource<T>(this);
        }
    }
}