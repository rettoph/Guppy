using Guppy.Core.Common;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Logging.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Configuration;
using Guppy.Core.Assets.Common.Constants;
using Guppy.Core.Assets.Common.Interfaces;
using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Core.Assets.Common.Services;
using Guppy.Core.Assets.Constants;

namespace Guppy.Core.Assets.Services
{
    public sealed class AssetPackService : IAssetPackService
    {
        private bool _initialized;
        private readonly IFileService _files;
        private readonly IDictionary<Guid, AssetPack> _packs;
        private readonly IFile<AssetPackCollectionConfiguration> _configuration;
        private readonly Lazy<IAssetTypeService> _resourceTypes;
        private readonly Lazy<ILogger> _logger;
        private readonly Lazy<ISettingService> _settingService;
        private readonly AssetPack _runtimeAssetPack;

        private SettingValue<string> _localization;

        public AssetPackService(
            IFileService files,
            IFiltered<AssetPackConfiguration> packs,
            IFiltered<IRuntimeAsset> runtimeAssetValues,
            Lazy<IAssetTypeService> resourceTypes,
            Lazy<ISettingService> settings,
            Lazy<ILogger> logger)
        {
            this._files = files;
            this._configuration = this._files.Get<AssetPackCollectionConfiguration>(
                path: new FilePath(DirectoryPath.AppData(string.Empty), FilePaths.AssetPacksConfiguration),
                createIfDoesNotExist: true);

            this._resourceTypes = resourceTypes;
            this._logger = logger;
            this._packs = new Dictionary<Guid, AssetPack>();
            this._settingService = settings;
            this._runtimeAssetPack = new AssetPack(Guid.Empty, "Runtime Assets", DirectoryPath.CurrentDirectory());

            this._configuration.Value = this._configuration.Value.AddRange(packs);
            this._files.Save(this._configuration);

            foreach (IRuntimeAsset runtimeAssetValue in runtimeAssetValues)
            {
                runtimeAssetValue.AddToPack(this._runtimeAssetPack);
            }
        }

        public void Initialize()
        {
            if (this._initialized == true)
            {
                return;
            }

            this._initialized = true;
            this._localization = this._settingService.Value.GetValue(GuppyAssetSettings.Localization);

            this._logger.Value.Debug("Preparing to import resource packs");

            bool saveConfigurationChanges = false;
            foreach (AssetPackConfiguration packConfiguration in this._configuration.Value.Packs)
            {
                if (packConfiguration.Enabled == true && this.TryLoad(packConfiguration) == false)
                {
                    this._logger.Value.Warning("Failed loading {AssetPack} at '{Directory}', disabling in configuration.", nameof(AssetPack), packConfiguration.EntryDirectory);
                    packConfiguration.Enabled = false;
                    saveConfigurationChanges = true;
                }
            }

            if (saveConfigurationChanges == true)
            {
                this._files.Save(this._configuration);
            }

            // Add the runtime resource pack last
            this._packs.Add(this._runtimeAssetPack.Id, this._runtimeAssetPack);

            this._logger.Value.Debug("Done. Imported ({Count}) resource packs", this._packs.Count);
        }

        public IEnumerable<IAssetPack> GetAll()
        {
            return this._packs.Values;
        }

        public IAssetPack GetById(Guid id)
        {
            return this._packs[id];
        }

        public IEnumerable<IAssetKey> GetDefinedAssets()
        {
            return this.GetAll().SelectMany(x => x.GetAllDefinedAssets()).Distinct();
        }

        public IEnumerable<T> GetDefinedValues<T>(AssetKey<T> key)
            where T : notnull
        {
            foreach (AssetPack pack in this.GetAll().Cast<AssetPack>())
            {
                if (pack.TryGetDefinedValue(key, GuppyAssetSettings.Localization.DefaultValue, out T? packValue))
                {
                    yield return packValue;
                }
            }

            if (this._localization != GuppyAssetSettings.Localization.DefaultValue)
            {
                foreach (AssetPack pack in this.GetAll().Cast<AssetPack>())
                {
                    if (pack.TryGetDefinedValue(key, this._localization, out T? packValue))
                    {
                        yield return packValue;
                    }
                }
            }
        }

        private AssetPack GetOrCreatePack(IFile<AssetPackEntryConfiguration> entry)
        {
            if (!this._packs.TryGetValue(entry.Value.Id, out var pack))
            {
                this._packs[entry.Value.Id] = pack = new AssetPack(
                    id: entry.Value.Id,
                    name: entry.Value.Name,
                    rootDirectory: entry.Path.DirectoryPath);
            }

            return pack;
        }

        private bool TryLoad(AssetPackConfiguration configuration)
        {
            try
            {
                FilePath entryLocation = new(configuration.EntryDirectory, "pack.json");
                IFile<AssetPackEntryConfiguration> entry = this._files.Get<AssetPackEntryConfiguration>(entryLocation);
                DirectoryPath directory = entry.Path.DirectoryPath;

                AssetPack pack = this.GetOrCreatePack(entry);
                this._logger.Value.Debug("Preparing to load resource pack {AssetPackName}, {AssetPackId} resources", pack.Name, pack.Id);

                foreach ((string localization, string[] resourceFileNames) in entry.Value.Import)
                {
                    foreach (string resourceFileName in resourceFileNames)
                    {
                        this.ImportAssetFile(resourceFileName, pack, directory, localization);
                    }
                }

                this._logger.Value.Debug("Done. Loaded ({Count}) resources", pack.GetAllDefinedAssets().Count());
                return true;
            }
            catch (Exception ex)
            {
                this._logger.Value.Error(ex, "Unable to load {AssetPack} at '{Directory}'", nameof(AssetPack), configuration.EntryDirectory);
                return false;
            }
        }

        private void ImportAssetFile(string resourceFileName, AssetPack pack, DirectoryPath directory, string localization)
        {
            FilePath resourceFileLocation = new(directory, resourceFileName);
            this._logger.Value.Verbose("Loading resource file {AssetFile}, {Localization}", resourceFileLocation, localization);

            IFile<AssetTypeValues[]> resourceTypeValuesFile = this._files.Get<AssetTypeValues[]>(resourceFileLocation);
            foreach (AssetTypeValues resourceTypeValues in resourceTypeValuesFile.Value)
            {
                this.ResolveAssetTypeValues(resourceTypeValues, pack, localization);
            }
        }

        private void ResolveAssetTypeValues(AssetTypeValues resourceTypeValues, AssetPack pack, string localization)
        {
            if (this._resourceTypes.Value.TryGet(resourceTypeValues.Type, out IAssetType? resourceType) == false)
            {
                this._logger.Value.Error("Unable to resolve resource type defined by {AssetName}, unknown.", resourceTypeValues.Type);
                return;
            }

            foreach (var (name, json) in resourceTypeValues.Values)
            {
                if (resourceType.TryResolve(pack, name, localization, json) == false)
                {
                    this._logger.Value.Error("Unable to resolve resource {AssetName}", name);
                    continue;
                }
            }
        }
    }
}