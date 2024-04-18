using Guppy.Core.Common.Contexts;
using Guppy.Engine.Common.Components;

namespace Guppy.Engine.Common
{
    public interface IGuppyEngine : IDisposable
    {
        IGuppyContext Context { get; }

        IEnumerable<IEngineComponent> Components { get; }

        IGuppyEngine Start();

        T Resolve<T>()
            where T : notnull;
    }
}
