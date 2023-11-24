using Guppy.Common.Providers;
using Guppy.Serialization;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common.Collections;
using System.Text.Json;
using Guppy.Files.Services;
using Guppy.Files.Enums;
using Guppy.Files;
using Guppy.Resources.Constants;
using Guppy.Resources.Loaders;

namespace Guppy.Resources.Providers
{
    internal sealed class SettingProvider : ISettingProvider, IDisposable
    {
        private readonly IFileService _files;
        private readonly Dictionary<Setting, SettingValue> _defaultValues;
        private readonly IFile<Dictionary<Setting, SettingValue>> _settings;

        public SettingProvider(IFileService files, IEnumerable<ISettingLoader> loaders)
        {
            _defaultValues = new Dictionary<Setting, SettingValue>();
            foreach (ISettingLoader loader in loaders)
            {
                loader.Load(this);
            }

            _files = files;
            _settings = _files.Get<Dictionary<Setting, SettingValue>>(FileType.AppData, FilePaths.Settings, true);
        }

        public void Register<T>(Setting<T> setting, T defaultValue) where T : notnull
        {
            SettingValue<T> defaultSettingValue = new SettingValue<T>(setting);
            defaultSettingValue.Value.Value = defaultValue;

            _defaultValues.Add(setting, defaultSettingValue);
        }

        public Ref<T> Get<T>(Setting<T> setting) where T : notnull
        {
            ref SettingValue? value = ref CollectionsMarshal.GetValueRefOrAddDefault(_settings.Value, setting, out bool exists);

            if(exists && value is SettingValue<T> casted)
            {
                return casted.Value;
            }

            if(_defaultValues.TryGetValue(setting, out value) && value is SettingValue<T> defaultCasted)
            {
                value = defaultCasted;

                return defaultCasted.Value;
            }

            value = casted = new SettingValue<T>(setting);

            return casted.Value;
        }

        public void Set<T>(Setting<T> setting, T value) where T : notnull
        {
            this.Get(setting).Value = value;
        }

        public void Dispose()
        {
            _files.Save(_settings);
        }

        public T GetDefault<T>(Setting<T> setting) where T : notnull
        {
            if (_defaultValues.TryGetValue(setting, out SettingValue? value) && value is SettingValue<T> casted)
            {
                return casted.Value;
            }

            throw new NotImplementedException();
        }
    }
}
