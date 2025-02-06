using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class GlobalSystemService(IEnumerable<IGlobalSystem> systems, IEnumerable<IGlobalSystemProvider> providers) : BaseSystemService<IGlobalSystem>(systems, providers), IGlobalSystemService
    {
    }
}
