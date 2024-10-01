using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Claims
{
    public abstract class Claim
    {
        public string Key { get; }
        public abstract ClaimType Type { get; }
        public ClaimAccessibility Accessibility { get; }
        public DateTime CreatedAt { get; internal set; }

        internal Claim(string key, ClaimAccessibility accessibility, DateTime? createdAt = null)
        {
            Key = key;
            Accessibility = accessibility;
            CreatedAt = createdAt ?? DateTime.UtcNow;
        }

        public abstract object? GetValue();

        public static Claim Create<T>(string key, T value, ClaimAccessibility accessibility)
        {
            return new Claim<T>(key, value, accessibility);
        }

        public static Claim Create<T>(T value, ClaimAccessibility accessibility)
        {
            return new Claim<T>(typeof(T).AssemblyQualifiedName ?? throw new Exception(), value, accessibility);
        }

        public static Claim Public<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibility.Public);
        }

        public static Claim Public<T>(T value)
        {
            return Create(value, ClaimAccessibility.Public);
        }

        public static Claim Protected<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibility.Protected);
        }

        public static Claim Protected<T>(T value)
        {
            return Create(value, ClaimAccessibility.Protected);
        }

        public static Claim Private<T>(string key, T value)
        {
            return Create(key, value, ClaimAccessibility.Private);
        }

        public static Claim Private<T>(T value)
        {
            return Create(value, ClaimAccessibility.Private);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Type.Name);

            writer.Put(Key);
            Type.SerializeValue(writer, GetValue());
            writer.Put(Accessibility);
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

    public class Claim<T>(string key, T value, ClaimAccessibility accessibility, DateTime? createdAt = null) : Claim(key, accessibility, createdAt)
    {
        public T Value { get; } = value;

        public override ClaimType<T> Type { get; } = ClaimType.Get<T>();

        public override object? GetValue()
        {
            return Value;
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
