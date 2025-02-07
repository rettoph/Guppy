using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Systems
{
    public interface IInitializeSystem : ISystem
    {
        [RequireSequenceGroup<InitializeSequenceGroupEnum>]
        void Initialize();
    }
}
