using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public class GlobalSystemService(
        Lazy<IEnumerable<IGlobalSystem>> systems,
        Lazy<IEnumerable<IGlobalSystemProvider>> providers
    ) : BaseSystemService<IGlobalSystem, IGlobalSystemProvider>(systems, providers), IGlobalSystemService
    {
    }
}
