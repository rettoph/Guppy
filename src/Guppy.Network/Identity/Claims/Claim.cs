using Guppy.Network.Identity.Enums;
using LiteNetLib.Utils;

namespace Guppy.Network.Identity.Claims
{
    public abstract class Claim
    {
        public string Key { get; }
        public abstract ClaimType Type { get; }
        public ClaimAccessibility Accessibility { get; }
        public DateTime CreatedAt { get; internal set; }

        internal Claim(string key, ClaimAccessibility accessibility)
        {
            this.Key = key;
            this.Accessibility = accessibility;
            this.CreatedAt = DateTime.UtcNow;
        }

        public abstract object? GetValue();

        public static Claim Create<T>(string key, T value, ClaimAccessibility accessibility)
        {
            return new Claim<T>(key, value, accessibility);
        }

        public static Claim Public<T>(string key, T value)
        {
            return Claim.Create(key, value, ClaimAccessibility.Public);
        }

        public static Claim Protected<T>(string key, T value)
        {
            return Claim.Create(key, value, ClaimAccessibility.Protected);
        }

        public static Claim Private<T>(string key, T value)
        {
            return Claim.Create(key, value, ClaimAccessibility.Private);
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
                accessibility: reader.GetEnum<ClaimAccessibility>());
        }

        public override bool Equals(object? obj)
        {
            return obj is Claim claim &&
                   Key == claim.Key &&
                   EqualityComparer<ClaimType>.Default.Equals(Type, claim.Type) &&
                   Accessibility == claim.Accessibility &&
                   CreatedAt == claim.CreatedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Type, Accessibility, CreatedAt);
        }
    }

    public class Claim<T> : Claim
    {
        public T Value { get; }

        public override ClaimType<T> Type { get; }

        internal Claim(string key, T value, ClaimAccessibility accessibility) : base(key, accessibility)
        {
            this.Value = value;
            this.Type = ClaimType.Get<T>();
        }

        public override object? GetValue()
        {
            return this.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Claim<T> claim &&
                   base.Equals(obj) &&
                   Key == claim.Key &&
                   EqualityComparer<ClaimType>.Default.Equals(Type, claim.Type) &&
                   Accessibility == claim.Accessibility &&
                   CreatedAt == claim.CreatedAt &&
                   EqualityComparer<T>.Default.Equals(Value, claim.Value) &&
                   EqualityComparer<ClaimType<T>>.Default.Equals(Type, claim.Type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Key, Type, Accessibility, CreatedAt, Value, Type);
        }
    }
}
