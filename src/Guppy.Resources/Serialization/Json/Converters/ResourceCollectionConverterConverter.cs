using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal class ResourceCollectionConverterConverter : JsonConverter<IResourceCollection>
    {
        public override IResourceCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var items = JsonSerializer.Deserialize<IEnumerable<IResource>>(ref reader, options);

            return new ResourceCollection((items ?? Enumerable.Empty<IResource>()).ToArray());
        }

        public override void Write(Utf8JsonWriter writer, IResourceCollection value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Items, options);
        }
    }
}
