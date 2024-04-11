using Guppy.Common;
using Guppy.Common.Utilities;
using Guppy.Resources.Utilities;
using Standart.Hash.xxHash;
using System.Runtime.InteropServices;

namespace Guppy.Resources
{
    public interface ISetting : IEquatable<ISetting>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }
        Type Type { get; }
        object DefaultValue { get; }
        object Value { get; set; }
    }

    public struct Setting<T> : ISetting, IRef<T>
        where T : notnull
    {
        private readonly int _defaultIndex;
        private readonly int _currentIndex;
        private readonly UnmanagedString _name;
        private readonly UnmanagedString _description;

        public readonly Guid Id;
        public string Name => _name.Value;
        public string Description => _description.Value;
        public Type Type => typeof(T);
        public T DefaultValue
        {
            get => StaticValueCollection<Setting<T>, T>.Get(_defaultIndex);
            set => StaticValueCollection<Setting<T>, T>.Set(_defaultIndex, value);
        }
        public T Value
        {
            get => StaticValueCollection<Setting<T>, T>.Get(_currentIndex);
            set => StaticValueCollection<Setting<T>, T>.Set(_currentIndex, value);
        }

        Guid ISetting.Id => this.Id;
        string ISetting.Name => this.Name;
        string ISetting.Description => this.Description;
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
            _defaultIndex = StaticValueCollection<Setting<T>, T>.Pop();
            _currentIndex = StaticValueCollection<Setting<T>, T>.Pop();
            _name = new UnmanagedString(name);
            _description = new UnmanagedString(description);

            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.DefaultValue = defaultValue;
            this.Value = defaultValue;
        }

        public void Dispose()
        {
            _cache.Remove(this.Name);

            StaticValueCollection<Setting<T>, T>.Push(_defaultIndex);
            StaticValueCollection<Setting<T>, T>.Push(_currentIndex);
            StaticCollection<ISetting>.Remove(this, false);

            _name.Dispose();
            _description.Dispose();
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

        public static void Clear()
        {
            while (_cache.Count > 0)
            {
                _cache.Values.First().Dispose();
            }

            _cache.Clear();

            StaticValueCollection<Setting<T>, T>.Clear();
        }
    }
}
