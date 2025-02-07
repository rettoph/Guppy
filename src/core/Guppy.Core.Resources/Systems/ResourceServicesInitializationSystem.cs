using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Resources.Services;

namespace Guppy.Core.Resources.Systems
{
    public class ResourceServicesInitializationSystem(
        ResourceService resourceService,
        ResourcePackService resourcePackService,
        SettingService settingService
    ) : IGlobalSystem,
        IInitializeSystem
    {
        private readonly ResourceService _resourceService = resourceService;
        private readonly ResourcePackService _resourcePackService = resourcePackService;
        private readonly SettingService _settingService = settingService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize()
        {
            this._settingService.Initialize();
            this._resourcePackService.Initialize();
            this._resourceService.Initialize();
        }
    }
}
