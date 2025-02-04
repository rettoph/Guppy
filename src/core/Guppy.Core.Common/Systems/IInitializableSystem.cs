using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Systems
{
    public interface IInitializableSystem<T> : ISystem
    {
        [RequireSequenceGroup<InitializeSystemSequenceGroupEnum>]
        void Initialize(T obj);
    }
}
