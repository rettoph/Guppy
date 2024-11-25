using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    public interface IEngineComponent
    {
        [RequireSequenceGroup<InitializeComponentSequenceGroup>]
        void Initialize(IGuppyEngine engine);
    }
}
