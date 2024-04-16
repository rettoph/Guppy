using Guppy.Core.Common.Attributes;
using Guppy.Game.ImGui.Common.Styling;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Game.ImGui.Common.ResourceTypes
{
    [AutoLoad]
    internal class ImStyleResourceType : DefaultResourceType<ImStyle>
    {
        public ImStyleResourceType(IJsonSerializationService json) : base(json)
        {
        }
    }
}
