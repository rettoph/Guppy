using Guppy.Common.Providers;
using Guppy.Resources.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public sealed class DictionaryPolymorphicConverter<T> : JsonConverter<Dictionary<Type, T>>
        where T : notnull
    {
        private IPolymorphicJsonSerializer<T> _serializer;

        public DictionaryPolymorphicConverter(IAssemblyProvider assembly, IPolymorphicJsonSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public override Dictionary<Type, T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<Type, T> result = new Dictionary<Type, T>();

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                T instance = _serializer.Deserialize(propertyName, ref reader, options, out Type type);
                reader.Read();

                result.Add(type, instance);
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Type, T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
