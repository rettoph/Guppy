using Autofac;

namespace Guppy.Common.Services
{
    public interface IServiceFilterService
    {
        bool Filter(ILifetimeScope scope, object service);
    }
}
