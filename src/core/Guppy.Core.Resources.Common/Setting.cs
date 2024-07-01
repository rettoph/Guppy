using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Resources.Common.Extensions.System;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Common
{
    public interface ISetting : IEquatable<ISetting>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; set; }
        Type Type { get; }
        object DefaultValue { get; }
        object Value { get; set; }
    }

    public struct Setting<T> : ISetting, IRef<T>
        where T : notnull
    {
        private readonly UnmanagedReference<ISetting, string> _name;
        private readonly UnmanagedReference<ISetting, string> _description;
        private readonly UnmanagedReference<ISetting, T> _default;
        private readonly UnmanagedReference<ISetting, T> _current;

        public readonly Guid Id;
        public string Name => _name.Value;
        public string Description
        {
            get => _description.Value;
            set => _description.SetValue(value);
        }
        public Type Type => typeof(T);
        public T DefaultValue
        {
            get => _default.Value;
            set => _default.SetValue(value);
        }
        public T Value
        {
            get => _current.Value;
            set => _current.SetValue(value);
        }

        Guid ISetting.Id => this.Id;
        string ISetting.Name => this.Name;
        string ISetting.Description
        {
            get => this.Description;
            set => this.Description = value;
        }
        Type ISetting.Type => this.Type;
        object ISetting.DefaultValue => this.DefaultValue;
        object ISetting.Value
        {
            get => this.Value;
            set
            {
                if (value is not T casted)
                {
                    throw new ArgumentException(nameof(value));
                }

                this.Value = casted;
            }
        }

        private unsafe Setting(string name, string description, T defaultValue)
        {
            _name = new UnmanagedReference<ISetting, string>(name);
            _description = new UnmanagedReference<ISetting, string>(description);
            _default = new UnmanagedReference<ISetting, T>(defaultValue);
            _current = new UnmanagedReference<ISetting, T>(defaultValue);

            this.Id = name.xxHash128();
        }

        public void Dispose()
        {
            StaticCollection<ISetting>.Remove(this, false);

            _cache.Remove(this.Name);

            _name.Dispose();
            _description.Dispose();
            _default.Dispose();
            _current.Dispose();
        }

        public override bool Equals(object? obj)
        {
            return Equals((Setting<T>)obj!);
        }

        public bool Equals(Setting<T> other)
        {
            return Id.Equals(other.Id);

        }

        public bool Equals(ISetting? other)
        {
            return other is Setting<T> casted
                && this.Equals(casted);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Setting<T> left, Setting<T> right)
        {
            return EqualityComparer<Setting<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(Setting<T> left, Setting<T> right)
        {
            return !(left == right);
        }

        public static implicit operator T(Setting<T> setting)
        {
            return setting.Value;
        }

        private static readonly Dictionary<string, Setting<T>> _cache = new Dictionary<string, Setting<T>>();
        public static Setting<T> Get(string name, string description, T defaultValue)
        {
            ref Setting<T> resource = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out bool exists);
            if (exists)
            {
                resource.Description = description;

                return resource;
            }

            resource = new Setting<T>(name, description, defaultValue);
            StaticCollection<ISetting>.Add(resource);

            return resource;
        }

        public static IEnumerable<Setting<T>> GetAll()
        {
            return _cache.Values;
        }
    }
}
