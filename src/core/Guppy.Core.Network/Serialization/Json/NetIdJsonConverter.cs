using System.Text.Json;
using System.Text.Json.Serialization;
using Guppy.Core.Network.Common;

namespace Guppy.Core.Network.Serialization.Json
{
    internal sealed class NetIdJsonConverter : JsonConverter<INetId>
    {
        private const string _typePropertyKey = "Type";
        private const string _valuePropertyKey = "Value";

        // private readonly Dictionary<string, Type> _implementationTypes;

        //public NetIdJsonConverter(IAssemblyService assembly)
        //{
        //    _implementationTypes = assembly.GetTypes<INetId>()
        //        .ToDictionary(
        //            keySelector: x => this.GetTypeKey(x),
        //            elementSelector: x => x);
        //}

        public override INetId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, INetId value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(_typePropertyKey, NetIdJsonConverter.GetTypeKey(value.GetType()));

            writer.WritePropertyName(_valuePropertyKey);
            JsonSerializer.Serialize(writer, value, value.GetType(), options);

            writer.WriteEndObject();
        }

        private static string GetTypeKey(Type type)
        {
            return type.Name ?? throw new MissingMemberException();
        }
    }
}