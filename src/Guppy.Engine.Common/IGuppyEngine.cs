using Guppy.Engine.Common.Contexts;

namespace Guppy.Engine.Common
{
    public interface IGuppyEngine
    {
        IGuppyContext Context { get; }

        T Resolve<T>()
            where T : notnull;
    }
}
