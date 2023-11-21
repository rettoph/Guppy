using Guppy.Resources.ResourceTypes;
using Guppy.GUI.Styling;
using Guppy.Resources;
using System.Text.Json;
using Guppy.Attributes;
using Guppy.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.GUI.ResourceTypes
{
    [AutoLoad]
    internal class StyleResourceType : ResourceType<Style>
    {
        private readonly IJsonSerializer _json;

        public StyleResourceType(IJsonSerializer json)
        {
            _json = json;
        }

        protected override bool TryResolve(Resource<Style> resource, string root, ref Utf8JsonReader reader, [MaybeNullWhen(false)] out Style value)
        {
            return _json.TryDeserialize<Style>(ref reader, out value);
        }
    }
}
