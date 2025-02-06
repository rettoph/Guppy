using Guppy.Core.Common.Systems;

namespace Guppy.Engine.Common.Systems
{
    public interface IEngineSystem : IGlobalSystem, IInitializeSystem<IGuppyEngine>
    {
    }
}