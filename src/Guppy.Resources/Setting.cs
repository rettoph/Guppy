using Guppy.Common.Collections;
using Standart.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public unsafe abstract class Setting : IEquatable<Setting?>
    {
        private static DoubleDictionary<Guid, string, Setting> _settings = new DoubleDictionary<Guid, string, Setting>();

        public readonly Guid Id;
        public readonly string Name;
        public readonly Type Type;

        public string? Description { get; internal set; }

        internal Setting(string name, Type type)
        {
            uint128 nameHash = xxHash128.ComputeHash(name);
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = name;
            this.Type = type;

            if (_settings.TryAdd(this.Id, this.Name, this) == false)
            {
                throw new NotImplementedException();
            }
        }

        internal static bool TryGet(string name, [MaybeNullWhen(false)] out Setting value)
        {
            return _settings.TryGet(name, out value);
        }

        public static Setting Get(Type type, string name)
        {
            if (_settings.TryGet(name, out Setting? cached))
            {
                return cached;
            }

            Type settingType = typeof(Setting<>).MakeGenericType(type);
            Setting setting = (Setting)Activator.CreateInstance(
                settingType, 
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { name },
                null)!;

            return setting;
        }

        public static Setting<T> Get<T>(string name, T defaultValue, string? description = null)
            where T : notnull
        {
            Setting<T> settingT = default!;

            if (_settings.TryGet(name, out Setting? Setting))
            {
                settingT = (Setting<T>)Setting;
            }
            else
            {
                settingT = new Setting<T>(name);
            }

            settingT.DefaultValue = defaultValue;
            settingT.Description = description;

            return settingT;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Setting);
        }

        public bool Equals(Setting? other)
        {
            return other is not null &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Setting? left, Setting? right)
        {
            return EqualityComparer<Setting>.Default.Equals(left, right);
        }

        public static bool operator !=(Setting? left, Setting? right)
        {
            return !(left == right);
        }
    }

    public sealed class Setting<T> : Setting
        where T : notnull
    {
        public T DefaultValue { get; internal set; }

        internal Setting(string name) : base(name, typeof(T))
        {
            this.DefaultValue = default!;
        }
    }
}
