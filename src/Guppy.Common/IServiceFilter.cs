using Autofac;

namespace Guppy.Common
{
    public interface IServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(ILifetimeScope scope);
    }
}
