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
    internal class ImStyleResourceType : DefaultResourceType<ImStyle>
    {
        public ImStyleResourceType(IJsonSerializer json) : base(json)
        {
        }
    }
}
