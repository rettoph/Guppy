using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    public interface IGuppyScope : IGuppyContainer, IDisposable
    {
        IGuppyRoot Root { get; }
        IScopeVariableService Variables { get; }
        IScopedSystemService Systems { get; }
    }
}
