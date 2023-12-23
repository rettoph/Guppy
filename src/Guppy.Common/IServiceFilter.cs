using Guppy.Common.Providers;

namespace Guppy.Common
{
    public interface IServiceFilter
    {
        bool AppliesTo(Type type);

        bool Invoke(IStateProvider state);
    }
}
