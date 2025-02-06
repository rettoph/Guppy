using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Systems
{
    public interface IInitializeSystem<T> : ISystem
    {
        [RequireSequenceGroup<InitializeSequenceGroupEnum>]
        void Initialize(T obj);
    }
}
