using Autofac;

namespace Guppy.Engine.Common.Services
{
    public interface IServiceFilterService
    {
        bool Filter(ILifetimeScope scope, object service);
    }
}
