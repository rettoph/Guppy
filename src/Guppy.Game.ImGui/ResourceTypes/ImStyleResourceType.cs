using Guppy.Attributes;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources.ResourceTypes;
using Guppy.Serialization;

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
