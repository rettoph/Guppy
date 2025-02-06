using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Systems
{
    public class ResourceServicesInitializationSystem(
        IResourceService resourceService,
        IResourcePackService resourcePackService,
        ISettingService settingService) : IGlobalSystem, IInitializeSystem<object>
    {
        private readonly IResourceService _resourceService = resourceService;
        private readonly IResourcePackService _resourcePackService = resourcePackService;
        private readonly ISettingService _settingService = settingService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize(object obj)
        {
            this._settingService.Initialize();
            this._resourcePackService.Initialize();
            this._resourceService.Initialize();
        }
    }
}
