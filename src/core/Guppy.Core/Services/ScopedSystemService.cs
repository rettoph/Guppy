using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class ScopedSystemService(IEnumerable<IScopedSystem> systems) : BaseSystemService<IScopedSystem>(systems), IScopedSystemService
    {
    }
}
