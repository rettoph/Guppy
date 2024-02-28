using Autofac;

namespace Guppy.Common.Providers
{
    public interface IServiceFilterProvider
    {
        bool Filter(ILifetimeScope scope, object service);
    }
}
