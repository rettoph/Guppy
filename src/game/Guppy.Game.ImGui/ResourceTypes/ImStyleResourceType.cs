using Guppy.Engine.Attributes;
using Guppy.Game.ImGui.Styling;
using Guppy.Core.Resources.ResourceTypes;
using Guppy.Engine.Serialization;

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
