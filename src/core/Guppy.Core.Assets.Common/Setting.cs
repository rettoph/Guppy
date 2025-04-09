using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Assets.Common.Extensions.System;

namespace Guppy.Core.Assets.Common
{
    public interface ISetting : IEquatable<ISetting>, IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }
        Type Type { get; }
        object DefaultValue { get; }
    }

    public static class Setting
    {
        private static readonly Dictionary<string, ISetting> _cache = [];

        public static bool TryGet(string name, Type type, [MaybeNullWhen(false)] out ISetting setting)
        {
            if (_cache.TryGetValue(name, out setting) == false)
            {
                return false;
            }

            if (type != setting.Type)
            {
                return false;
            }

            return true;
        }

        public static ISetting Get(string name, Type type)
        {
            if (_cache.TryGetValue(name, out ISetting? setting) == false)
            {
                throw new NotImplementedException();
            }

            if (type != setting.Type)
            {
                throw new NotImplementedException();
            }

            return setting;
        }

        public static ISetting Get(string name, string description, Type type, object? defaultValue = null)
        {
            MethodInfo setterGetterInfo = typeof(Setting<>).MakeGenericType(type).GetMethod(nameof(Setting<object>.Get), BindingFlags.Static | BindingFlags.Public) ?? throw new NotImplementedException();
            ISetting setting = setterGetterInfo.Invoke(null, [name, description, defaultValue]) as ISetting ?? throw new NotImplementedException();

            return setting;
        }

        public static IEnumerable<ISetting> GetAll()
        {
            return _cache.Values;
        }

        public static IEnumerable<ISetting> GetAll(Type type)
        {
            return _cache.Values.Where(x => x.Type == type);
        }

        internal static void CacheRemove(string name)
        {
            _cache.Remove(name);
        }

        internal static ref ISetting? CacheGetOrAdd(string name, out bool exists)
        {
            ref ISetting? setting = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, name, out exists);
            return ref setting;
        }
    }

    public readonly struct Setting<T> : ISetting
        where T : notnull
    {
        private readonly UnmanagedReference<Setting<T>, string> _name;
        private readonly UnmanagedReference<Setting<T>, string> _description;
        private readonly UnmanagedReference<Setting<T>, T> _default;

        public readonly Guid Id;
        public string Name => this._name.Value;
        public readonly string Description
        {
            get => this._description.Value;
        }
        public readonly Type Type => typeof(T);
        public readonly T DefaultValue
        {
            get => this._default.Value;
            set => this._default.SetValue(value);
        }

        readonly Guid ISetting.Id => this.Id;
        string ISetting.Name => this.Name;

        readonly string ISetting.Description
        {
            get => this.Description;
        }
        Type ISetting.Type => this.Type;

        readonly object ISetting.DefaultValue
        {
            get => this.DefaultValue;
        }

        private unsafe Setting(string name, string description, T defaultValue)
        {
            this._name = new UnmanagedReference<Setting<T>, string>(name);
            this._description = new UnmanagedReference<Setting<T>, string>(description);
            this._default = new UnmanagedReference<Setting<T>, T>(defaultValue);

            this.Id = $"{typeof(T).FullName}.{name}".xxHash128();
        }

        public void Dispose()
        {
            StaticCollection<ISetting>.Remove(this, false);

            Setting.CacheRemove(this.Name);

            this._name.Dispose();
            this._description.Dispose();
            this._default.Dispose();
        }

        public override readonly bool Equals(object? obj)
        {
            return this.Equals((Setting<T>)obj!);
        }

        public readonly bool Equals(Setting<T> other)
        {
            return this.Id.Equals(other.Id);
        }

        public readonly bool Equals(ISetting? other)
        {
            return other is Setting<T> casted
                && this.Equals(casted);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Id);
        }

        public static bool operator ==(Setting<T> left, Setting<T> right)
        {
            return EqualityComparer<Setting<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(Setting<T> left, Setting<T> right)
        {
            return !(left == right);
        }

        public static Setting<T> Get(string name, string description, T defaultValue)
        {
            ref ISetting? cached = ref Setting.CacheGetOrAdd(name, out bool exists);

            if (exists == false)
            {
                Setting<T> setting = new(name, description, defaultValue);
                cached = setting;
                return setting;
            }

            if (cached is not Setting<T> casted)
            {
                throw new InvalidOperationException($"Preexisting setting '{cached!.Name}' type mismatch. Expected {typeof(T).Name} but got {cached!.Type.Name}");
            }

            return casted;
        }

        public static IEnumerable<Setting<T>> GetAll()
        {
            return Setting.GetAll().OfType<Setting<T>>();
        }
    }
}