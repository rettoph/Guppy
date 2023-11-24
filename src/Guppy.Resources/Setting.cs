using Guppy.Common.Collections;
using Standart.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public unsafe abstract class Setting : IEquatable<Setting?>
    {
        private static DoubleDictionary<Guid, string, Setting> _settings = new DoubleDictionary<Guid, string, Setting>();

        public readonly Guid Id;
        public readonly string Name;
        public readonly Type Type;

        internal Setting(string name, Type type)
        {
            uint128 nameHash = xxHash128.ComputeHash($"{type.AssemblyQualifiedName}##{name}");
            Guid* pNameHash = (Guid*)&nameHash;
            this.Id = pNameHash[0];
            this.Name = name;

            _settings.TryAdd(this.Id, this.Name, this);
            Type = type;
        }

        public static Setting Get(Guid id)
        {
            return _settings[id];
        }

        public static Setting<T> Define<T>(string name, T defaultValue)
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
        public T DefaultValue;

        internal Setting(string name) : base(name, typeof(T))
        {
            this.DefaultValue = default!;
        }
    }
}
