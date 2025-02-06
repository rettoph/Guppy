using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class ScopedSystemService(IEnumerable<IScopedSystem> systems, IEnumerable<IScopedSystemProvider> providers) : BaseSystemService<IScopedSystem>(systems, providers), IScopedSystemService
    {
    }
}
