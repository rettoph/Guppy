using System.Reflection;

namespace Guppy.Engine.Common.Contexts
{
    public interface IGuppyContext
    {
        string Company { get; }
        string Name { get; }
        Assembly Entry { get; }
        IEnumerable<Assembly> Libraries { get; }
    }
}
