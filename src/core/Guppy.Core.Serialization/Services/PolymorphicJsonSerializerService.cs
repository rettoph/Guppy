using System.Text.Json;
using Autofac;
using Guppy.Core.Common.Collections;
using Guppy.Core.Resources.Serialization.Json;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Serialization.Services
{
    internal class PolymorphicJsonSerializerService<TBase> : IPolymorphicJsonSerializerService<TBase>
    {
        private readonly Map<string, Type> _types;

        public PolymorphicJsonSerializerService(IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = types.Where(x => x.BaseType.IsAssignableTo<TBase>()).Select(x => (x.Key, x.InstanceType));
            this._types = new Map<string, Type>(typeTuples);
        }

        public TBase Deserialize(string key, ref JsonElement element, JsonSerializerOptions options, out Type type)
        {
            type = this._types[key];
            object? instance = JsonSerializer.Deserialize(element, type, options);
            TBase casted = (TBase?)instance ?? throw new NotImplementedException();

            return casted;
        }

        public TBase Deserialize(string key, ref Utf8JsonReader reader, JsonSerializerOptions options, out Type type)
        {
            type = this._types[key];
            TBase instance = (TBase)JsonSerializer.Deserialize(ref reader, type, options)!;

            return instance;
        }

        public string GetKey(Type type)
        {
            return this._types[type];
        }

        public Type GetType(string key)
        {
            return this._types[key];
        }
    }
}