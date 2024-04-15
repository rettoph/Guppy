using Autofac;

namespace Guppy.Core.Common.Services
{
    public interface IServiceFilterService
    {
        bool Filter(ILifetimeScope scope, object service);
    }
}
