using Autofac;

namespace Guppy.Core.Common.Services
{
    public interface IServiceFilterService
    {
        IServiceFilter[] GetFilters(Type type);

        bool Filter(ILifetimeScope scope, object service);
    }
}