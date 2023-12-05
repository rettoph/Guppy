using Guppy.Game.ImGui.Styling.StyleValueResources;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
            ImGuiCol col = default;
            Resource<Color> resource = default!;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? property))
            {
                switch (property)
                {
                    case nameof(ImStyleColorValue.Property):
                        col = Enum.Parse<ImGuiCol>(reader.ReadString());
                        break;

                    case nameof(ImStyleColorValue.Color):
                        resource = Resources.Resource.Get<Color>(reader.ReadString());
                        break;
                }
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            IResourceProvider resources = _resources.Value;
            return new ImStyleColorValue(col, resources.Get(resource));
        }

        public override void Write(Utf8JsonWriter writer, ImStyleColorValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
