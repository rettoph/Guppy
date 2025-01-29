using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Common
{
    public interface IGuppyEngine : IDisposable
    {
        IEnumerable<IEngineComponent> Components { get; }

        IGuppyEngine Start();

        T Resolve<T>()
            where T : notnull;
    }
}