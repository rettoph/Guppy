using Autofac;

namespace Guppy.Core
{
    internal sealed class Lazier<T> : Lazy<T>
        where T : class
    {
        public Lazier(IComponentContext p) : base(() => p.Resolve<T>())
        {

        }
    }
}
