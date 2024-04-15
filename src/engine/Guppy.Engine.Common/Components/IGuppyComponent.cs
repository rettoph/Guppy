using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Common.Components
{
    [Service<IGuppyComponent>(ServiceLifetime.Scoped, true, tag: LifetimeScopeTags.GuppyScope)]
    public interface IGuppyComponent : ISequenceable<InitializeSequence>
    {
        void Initialize(IGuppy guppy);
    }
}
