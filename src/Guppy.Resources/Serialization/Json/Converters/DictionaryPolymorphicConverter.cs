using Guppy.Common.Providers;
using Guppy.Resources.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public sealed class DictionaryPolymorphicConverter<T> : JsonConverter<Dictionary<string, T>>
        where T : notnull
    {
        private IPolymorphicJsonSerializer<T> _serializer;

        public DictionaryPolymorphicConverter(IAssemblyProvider assembly, IPolymorphicJsonSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public override Dictionary<string, T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                T instance = _serializer.Deserialize(propertyName, ref reader, options);
                reader.Read();

                result.Add(propertyName, instance);
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
