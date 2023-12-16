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

        internal abstract ISettingValue Deserialize(string name, ref Utf8JsonReader reader, JsonSerializerOptions options);

        internal abstract void Serialize(Utf8JsonWriter writer, ISettingValue settingValue, JsonSerializerOptions options);

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

        public static Setting<T> Get<T>(string name)
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
        internal Setting(string name) : base(name, typeof(T))
        {
        }

        internal override ISettingValue Deserialize(string name, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Setting<T> setting = Setting.Get<T>(name);
            T value = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new NotImplementedException();

            SettingValue<T> settingValue = new SettingValue<T>(setting, value);

            return settingValue;
        }

        internal override void Serialize(Utf8JsonWriter writer, ISettingValue settingValue, JsonSerializerOptions options)
        {
            if (settingValue is SettingValue<T> casted)
            {
                writer.WriteStartObject();

                writer.WriteString(nameof(Setting.Name), casted.Setting.Name);
                writer.WriteString(nameof(Setting.Type), casted.Setting.Type.Name);

                writer.WritePropertyName(nameof(SettingValue<T>.Value));
                JsonSerializer.Serialize(writer, casted.Value, options);

                writer.WriteEndObject();
            }
        }
    }
}
