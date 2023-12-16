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
        private readonly Dictionary<Setting, ISettingValue> _values;
        private readonly IFile<Dictionary<Setting, ISettingValue>> _file;

        public SettingProvider(IFileService files)
        {
            _values = new Dictionary<Setting, ISettingValue>();
            _files = files;
            _file = _files.Get<Dictionary<Setting, ISettingValue>>(FileType.AppData, FilePaths.Settings, true);

            this.UpdateValues(_file.Value);
            this.Save();
        }

        public SettingValue<T> Get<T>(Setting<T> setting) where T : notnull
        {
            ref ISettingValue? value = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, setting, out bool exists);

            if(exists && value is SettingValue<T> casted)
            {
                return casted;
            }

            throw new KeyNotFoundException(setting.Name);
        }

        public void Set<T>(Setting<T> setting, T value) where T : notnull
        {
            this.Get(setting).Value = value;
        }

        public void Save()
        {
            _file.Value = _values;
            _files.Save(_file);
        }

        public void Dispose()
        {
            this.Save();
        }

        private void UpdateValues(Dictionary<Setting, ISettingValue> values)
        {
            foreach(var (setting, value) in values)
            {
                ref ISettingValue? settingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, setting, out bool exists);

                if(exists == false)
                {
                    Type settingValueType = typeof(SettingValue<>).MakeGenericType(setting.Type);
                    settingValue = (ISettingValue)Activator.CreateInstance(settingValueType, setting)!;
                }

                settingValue!.Value = value.Value;
            }
        }
    }
}
