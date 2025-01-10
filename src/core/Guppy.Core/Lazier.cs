using Autofac;

namespace Guppy.Core
{
    internal sealed class Lazier<T>(IComponentContext p) : Lazy<T>(p.Resolve<T>)
        where T : class
    {
    }
}