using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class GlobalSystemService(IEnumerable<IGlobalSystem> systems) : BaseSystemService<IGlobalSystem>(systems), IGlobalSystemService
    {
    }
}
