using Guppy.Common.Providers;
using Guppy.Serialization;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Guppy.Resources.Serialization.Settings;
using Guppy.Common.Collections;
using System.Text.Json;
using Guppy.Files.Services;
using Guppy.Files.Enums;
using Guppy.Files;
using Guppy.Resources.Constants;

namespace Guppy.Resources.Providers
{
    internal sealed class SettingProvider : ISettingProvider, IDisposable
    {
        private readonly IFileService _files;
        private readonly IFile<Dictionary<Setting, SettingValue>> _settings;

        public SettingProvider(IFileService files, IJsonSerializer json)
        {
            _files = files;
            _settings = _files.Get<Dictionary<Setting, SettingValue>>(FileType.AppData, FilePaths.Settings, true);
        }

        public Ref<T> Get<T>(Setting<T> setting) where T : notnull
        {
            ref SettingValue? value = ref CollectionsMarshal.GetValueRefOrAddDefault(_settings.Value, setting, out bool exists);

            if(exists && value is SettingValue<T> casted)
            {
                return casted.Value;
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
    }
}
