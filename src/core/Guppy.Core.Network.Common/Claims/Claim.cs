using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Claims
{
    public abstract class Claim
    {
        public string Key { get; }
        public abstract ClaimType Type { get; }
        public ClaimAccessibilityEnum Accessibility { get; }
        public DateTime CreatedAt { get; internal set; }

        internal Claim(string key, ClaimAccessibilityEnum accessibility, DateTime? createdAt = null)
        {
            this.Key = key;
            this.Accessibility = accessibility;
            this.CreatedAt = createdAt ?? DateTime.UtcNow;
        }

        public abstract object? GetValue();

        public static Claim Create<T>(string key, T value, ClaimAccessibilityEnum accessibility)
        {
            return new Claim<T>(key, value, accessibility);
        }

        public static Claim Create<T>(T value, ClaimAccessibilityEnum accessibility)
        {
            return new Claim<T>(typeof(T).AssemblyQualifiedName ?? throw new Exception(), value, accessibility);
        }

        public static Claim Public<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibilityEnum.Public);
        }

        public static Claim Public<T>(T value)
        {
            return Create(value, ClaimAccessibilityEnum.Public);
        }

        public static Claim Protected<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibilityEnum.Protected);
        }

        public static Claim Protected<T>(T value)
        {
            return Create(value, ClaimAccessibilityEnum.Protected);
        }

        public static Claim Private<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibilityEnum.Private);
        }

        public static Claim Private<T>(T value)
        {
            return Create(value, ClaimAccessibilityEnum.Private);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(this.Type.Name);

            writer.Put(this.Key);
            this.Type.SerializeValue(writer, this.GetValue());
            writer.Put(this.Accessibility);
        }

        public static Claim Deserialize(NetDataReader reader)
        {
            var type = ClaimType.Get(reader.GetString());

            return type.Create(
                key: reader.GetString(),
                value: type.DeserializeValue(reader),
                accessibility: reader.GetEnum<ClaimAccessibilityEnum>());
        }

        public override bool Equals(object? obj)
        {
            return obj is Claim claim &&
                   this.Key == claim.Key &&
                   EqualityComparer<ClaimType>.Default.Equals(this.Type, claim.Type) &&
                   this.Accessibility == claim.Accessibility &&
                   this.CreatedAt == claim.CreatedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Key, this.Type, this.Accessibility, this.CreatedAt);
        }
    }

    public class Claim<T>(string key, T value, ClaimAccessibilityEnum accessibility, DateTime? createdAt = null) : Claim(key, accessibility, createdAt)
    {
        public T Value { get; } = value;

        public override ClaimType<T> Type { get; } = ClaimType.Get<T>();

        public override object? GetValue()
        {
            return this.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Claim<T> claim &&
                   base.Equals(obj) &&
                   this.Key == claim.Key &&
                   EqualityComparer<ClaimType>.Default.Equals(this.Type, claim.Type) &&
                   this.Accessibility == claim.Accessibility &&
                   this.CreatedAt == claim.CreatedAt &&
                   EqualityComparer<T>.Default.Equals(this.Value, claim.Value) &&
                   EqualityComparer<ClaimType<T>>.Default.Equals(this.Type, claim.Type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.Key, this.Type, this.Accessibility, this.CreatedAt, this.Value, this.Type);
        }
    }
}