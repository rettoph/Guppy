using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class ScopedSystemService(
        Lazy<IEnumerable<IScopedSystem>> systems,
        Lazy<IEnumerable<IScopedSystemProvider>> providers
    ) : BaseSystemService<IScopedSystem, IScopedSystemProvider>(systems, providers),
        IScopedSystemService
    {
    }
}
