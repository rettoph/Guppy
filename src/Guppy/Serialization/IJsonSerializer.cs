using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        void Serialize<T>(Stream utf8Json, T obj);
        void Serialize<T>(Utf8JsonWriter writer, T obj);

        T? Deserialize<T>(string json, out bool success);
        T? Deserialize<T>(Stream utf8Json, out bool success);
        T? Deserialize<T>(ref Utf8JsonReader reader, out bool success);
    }
}
