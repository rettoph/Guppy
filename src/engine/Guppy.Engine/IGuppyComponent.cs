using Guppy.Engine.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Enums;

namespace Guppy.Engine
{
    [Service<IGuppyComponent>(ServiceLifetime.Scoped, true, tag: LifetimeScopeTags.GuppyScope)]
    public interface IGuppyComponent : ISequenceable<InitializeSequence>
    {
        void Initialize(IGuppy guppy);
    }
}
