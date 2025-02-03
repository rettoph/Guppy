using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common
{
    public interface ISystem
    {
    }
    public interface ISystem<T> : ISystem
    {
        [RequireSequenceGroup<InitializeSystemSequenceGroupEnum>]
        void Initialize(T obj);
    }
}
