using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Resources;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Game.ImGui.Serialization.Json.Converters
{
    internal class ImStyleColorValueConverter : JsonConverter<ImStyleColorValue>
    {
        private readonly Lazy<IResourceProvider> _resources;

        public ImStyleColorValueConverter(Lazy<IResourceProvider> resources)
        {
            _resources = resources;
        }

        public override ImStyleColorValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = null;
            ImGuiCol col = default;
            Resource<Color> resource = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleValue.Key):
                        key = reader.ReadString();
                        break;

                    case nameof(ImStyleColorValue.Property):
                        col = Enum.Parse<ImGuiCol>(reader.ReadString());
                        break;

                    case nameof(ImStyleColorValue.Color):
                        resource = Resource.Get<Color>(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            IResourceProvider resources = _resources.Value;
            return new ImStyleColorValue(key, col, resources.Get(resource));
        }

        public override void Write(Utf8JsonWriter writer, ImStyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
