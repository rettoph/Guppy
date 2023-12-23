using Guppy.Common.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Network.Serialization.Json
{
    internal sealed class NetIdJsonConverter : JsonConverter<INetId>
    {
        private const string TypePropertyKey = "Type";
        private const string ValuePropertyKey = "Value";

        private Dictionary<string, Type> _implementationTypes;

        public NetIdJsonConverter(IAssemblyProvider assembly)
        {
            _implementationTypes = assembly.GetTypes<INetId>()
                .ToDictionary(
                    keySelector: x => this.GetTypeKey(x),
                    elementSelector: x => x);
        }

        public override INetId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, INetId value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(TypePropertyKey, this.GetTypeKey(value.GetType()));

            writer.WritePropertyName(ValuePropertyKey);
            JsonSerializer.Serialize(writer, value, value.GetType(), options);

            writer.WriteEndObject();
        }

        private string GetTypeKey(Type type)
        {
            return type.Name ?? throw new MissingMemberException();
        }
    }
}
