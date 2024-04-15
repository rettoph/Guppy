using Autofac;

namespace Guppy.Core.Common
{
    public interface IServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(ILifetimeScope scope);
    }
}
