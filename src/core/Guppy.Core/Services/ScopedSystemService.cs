using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public class ScopedSystemService(IEnumerable<IScopedSystem> systems) : BaseSystemService<IScopedSystem>(systems), IScopedSystemService
    {
    }
}
