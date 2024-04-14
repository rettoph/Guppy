using Autofac;

namespace Guppy.Engine.Common
{
    public interface IServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(ILifetimeScope scope);
    }
}
