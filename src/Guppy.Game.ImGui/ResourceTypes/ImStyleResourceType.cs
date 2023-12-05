using Guppy.Resources.ResourceTypes;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using System.Text.Json;
using Guppy.Attributes;
using Guppy.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.ImGui.ResourceTypes
{
    [AutoLoad]
    internal class ImStyleResourceType : ResourceType<ImStyle>
    {
        private readonly IJsonSerializer _json;

        public ImStyleResourceType(IJsonSerializer json)
        {
            _json = json;
        }

        protected override bool TryResolve(Resource<ImStyle> resource, string root, ref Utf8JsonReader reader, [MaybeNullWhen(false)] out ImStyle value)
        {
            return _json.TryDeserialize<ImStyle>(ref reader, out value);
        }
    }
}
