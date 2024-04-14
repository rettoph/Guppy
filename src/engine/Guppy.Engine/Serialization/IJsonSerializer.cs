using System.Text.Json;

namespace Guppy.Engine.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        void Serialize<T>(Stream utf8Json, T obj);
        void Serialize<T>(Utf8JsonWriter writer, T obj);

        T Deserialize<T>(string json, out bool success);
        T Deserialize<T>(Stream utf8Json, out bool success);
        T Deserialize<T>(ref Utf8JsonReader reader, out bool success);
        T Deserialize<T>(ref JsonElement json, out bool success);
    }
}
