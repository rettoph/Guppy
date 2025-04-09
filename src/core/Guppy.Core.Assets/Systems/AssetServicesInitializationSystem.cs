using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Assets.Services;

namespace Guppy.Core.Assets.Systems
{
    public class AssetServicesInitializationSystem(
        AssetService assetService,
        AssetPackService resourcePackService,
        SettingService settingService
    ) : IGlobalSystem,
        IInitializeSystem
    {
        private readonly AssetService _assetService = assetService;
        private readonly AssetPackService _resourcePackService = resourcePackService;
        private readonly SettingService _settingService = settingService;

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.PreInitialize)]
        public void Initialize()
        {
            this._settingService.Initialize();
            this._resourcePackService.Initialize();
            this._assetService.Initialize();
        }
    }
}
