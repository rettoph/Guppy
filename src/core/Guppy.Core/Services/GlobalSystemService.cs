using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public class GlobalSystemService(IEnumerable<IGlobalSystem> systems) : BaseSystemService<IGlobalSystem>(systems), IGlobalSystemService
    {
    }
}
