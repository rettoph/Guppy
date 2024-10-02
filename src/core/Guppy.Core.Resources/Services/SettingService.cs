using Guppy.Core.Common.Extensions.System;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Constants;
using Guppy.Core.Resources.Common.Services;
using Serilog;
using System.Runtime.InteropServices;

namespace Guppy.Core.Resources.Services
{
    internal sealed class SettingService(IFileService files, ILogger logger) : IHostedService, ISettingService, IDisposable
    {
        private bool _initialized;
        private readonly IFileService _files = files;
        private IFile<IEnumerable<ISettingValue>> _file = null!;
        private Dictionary<Guid, ISettingValue> _values = null!;
        private readonly ILogger _logger = logger;

        public ISettingValue this[ISetting setting] => _values[setting.Id];

        public Task StartAsync(CancellationToken cancellation)
        {
            this.Initialize();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            FileLocation location = new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.Settings);
            _logger.Debug("{ClassName}::{MethodName} - Preparing to import setting values from '{SettingFileLocation}'", nameof(SettingService), nameof(Initialize), location);

            _file = _files.Get<IEnumerable<ISettingValue>>(location, true);
            _values = _file.Value.ToDictionary(x => x.Setting.Id, x => x);

            _initialized = true;

            _logger.Debug("{ClassName}::{MethodName} - Done. Imported ({Count}) values", nameof(SettingService), nameof(Initialize), _values.Count);
            foreach (ISettingValue value in _values.Values)
            {
                _logger.Verbose("{ClassName}::{MethodName} - Setting = {Setting}, Type = {Type}, Value = {Value}", nameof(SettingService), nameof(Initialize), value.Setting.Name, value.Type.GetFormattedName(), value.Value);
            }
        }

        public void Save()
        {
            _file.Value = _values.Values;
            _files.Save(_file);
        }

        public void Dispose()
        {
            this.Save();

            StaticCollection<ISetting>.Clear(true);
        }

        public SettingValue<T> GetValue<T>(Setting<T> setting) where T : notnull
        {
            ref ISettingValue? cache = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, setting.Id, out bool exists);
            if (exists == true)
            {
                return (SettingValue<T>)cache!;
            }

            SettingValue<T> value = new SettingValue<T>(setting);
            cache = value;

            return value;
        }

        public ISettingValue GetValue(ISetting setting)
        {
            return _values[setting.Id];
        }
    }
}
