using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    public interface IInitializableComponent
    {
        [RequireSequenceGroup<InitializeComponentSequenceGroup>]
        void Initialize(IGuppyEngine engine);
    }
}
