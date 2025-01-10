using System.Runtime.InteropServices;
using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Core.Resources.Constants;
using Serilog;

namespace Guppy.Core.Resources.Services
{
    internal sealed class SettingService(IFileService files, ILogger logger) : IHostedService, ISettingService, IDisposable
    {
        private bool _initialized;
        private readonly IFileService _files = files;
        private IFile<IEnumerable<ISettingValue>> _file = null!;
        private Dictionary<Guid, ISettingValue> _values = null!;
        private readonly ILogger _logger = logger;

        public ISettingValue this[ISetting setting] => this._values[setting.Id];

        public Task StartAsync(CancellationToken cancellation)
        {
            this.Initialize();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellation) => Task.CompletedTask;

        public void Initialize()
        {
            if (this._initialized)
            {
                return;
            }

            FileLocation location = new(DirectoryLocation.AppData(string.Empty), FilePaths.Settings);
            this._logger.Debug("Preparing to import setting values from '{SettingFileLocation}'", location);

            this._file = this._files.Get<IEnumerable<ISettingValue>>(location, true);
            this._values = this._file.Value.ToDictionary(x => x.Setting.Id, x => x);

            this._initialized = true;

            this._logger.Debug("Done. Imported ({Count}) values", this._values.Count);
            foreach (ISettingValue value in this._values.Values)
            {
                this._logger.Verbose("Setting = {Setting}, Type = {Type}, Value = {Value}", value.Setting.Name, value.Type.GetFormattedName(), value.Value);
            }
        }

        public void Save()
        {
            this._file.Value = this._values.Values;
            this._files.Save(this._file);
        }

        public void Dispose()
        {
            this.Save();

            StaticCollection<ISetting>.Clear(true);
        }

        public SettingValue<T> GetValue<T>(Setting<T> setting) where T : notnull
        {
            ref ISettingValue? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(this._values, setting.Id, out bool exists);
            if (exists == true)
            {
                return (SettingValue<T>)cache!;
            }

            SettingValue<T> value = new(setting);
            cache = value;

            return value;
        }

        public ISettingValue GetValue(ISetting setting) => this._values[setting.Id];
    }
}