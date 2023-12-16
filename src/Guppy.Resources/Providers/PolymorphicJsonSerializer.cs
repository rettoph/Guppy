using Guppy.Common.Collections;
using Guppy.Resources.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    internal class PolymorphicJsonSerializer<TBase> : IPolymorphicJsonSerializer<TBase>
    {
        private readonly Map<string, Type> _types;

        public PolymorphicJsonSerializer(IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = types.Where(x => x.BaseType == typeof(TBase)).Select(x => (x.Key, x.InstanceType));
            _types = new Map<string, Type>(typeTuples);
        }

        public TBase Deserialize(string key, ref JsonElement element, JsonSerializerOptions options)
        {
            Type type = _types[key];
            TBase instance = (TBase)JsonSerializer.Deserialize(element, type, options)!;

            return instance;
        }

        public TBase Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            Type type = _types[key];
            TBase instance = (TBase)JsonSerializer.Deserialize(ref reader, type, options)!;

            return instance;
        }

        public string GetKey(Type type)
        {
            return _types[type];
        }
        public Type GetImplementationType(string key)
        {
            return _types[key];
        }
    }
}
