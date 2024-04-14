using Guppy.Common.Contexts;

namespace Guppy.Common
{
    public interface IGuppyEngine
    {
        IGuppyContext Context { get; }

        T Resolve<T>()
            where T : notnull;
    }
}
