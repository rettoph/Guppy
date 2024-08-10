using Autofac;
using Guppy.Core.Common.Collections;
using Guppy.Core.Resources.Serialization.Json;
using Guppy.Core.Serialization.Common.Services;
using System.Text.Json;

namespace Guppy.Core.Serialization.Services
{
    internal class PolymorphicJsonSerializerService<TBase> : IPolymorphicJsonSerializerService<TBase>
    {
        private readonly Map<string, Type> _types;

        public PolymorphicJsonSerializerService(IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = types.Where(x => x.BaseType.IsAssignableTo<TBase>()).Select(x => (x.Key, x.InstanceType));
            _types = new Map<string, Type>(typeTuples);
        }

        public TBase Deserialize(string key, ref JsonElement element, JsonSerializerOptions options, out Type type)
        {
            type = _types[key];
            TBase instance = (TBase)JsonSerializer.Deserialize(element, type, options)!;

            return instance;
        }

        public TBase Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options, out Type type)
        {
            type = _types[key];
            TBase instance = (TBase)JsonSerializer.Deserialize(ref reader, type, options)!;

            return instance;
        }

        public string GetKey(Type type)
        {
            return _types[type];
        }

        public Type GetType(string key)
        {
            return _types[key];
        }
    }
}
