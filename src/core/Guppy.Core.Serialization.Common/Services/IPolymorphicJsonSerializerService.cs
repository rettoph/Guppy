using System.Text.Json;

namespace Guppy.Core.Serialization.Common.Services
{
    public interface IPolymorphicJsonSerializerService<TBase>
    {
        string GetKey(Type implementationType);
        Type GetImplementationType(string key);

        TBase Deserialize(string key, ref JsonElement element, JsonSerializerOptions options, out Type type);
        TBase Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options, out Type type);
    }
}
