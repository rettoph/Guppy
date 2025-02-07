using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Systems
{
    public interface IDeinitializeSystem<T> : ISystem
    {
        [RequireSequenceGroup<DeinitializeSequenceGroupEnum>]
        void Deinitialize(T obj);
    }
}
