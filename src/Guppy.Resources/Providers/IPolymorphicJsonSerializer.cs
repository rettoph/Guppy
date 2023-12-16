using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IPolymorphicJsonSerializer<TBase>
    {
        string GetKey(Type implementationType);
        Type GetImplementationType(string key);

        TBase Deserialize(string key, ref JsonElement element, JsonSerializerOptions options);
        TBase Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options);
    }
}
