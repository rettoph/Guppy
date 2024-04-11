using Guppy.Files;
using Guppy.Files.Services;
using Guppy.Resources.Constants;
using System.Runtime.InteropServices;

namespace Guppy.Resources.Services
{
    internal sealed class SettingService : ISettingService, IDisposable
    {
        private readonly IFileService _files;
        private readonly Dictionary<Setting, ISettingValue> _values;
        private readonly IFile<Dictionary<Setting, ISettingValue>> _file;

        public SettingService(IFileService files)
        {
            _values = new Dictionary<Setting, ISettingValue>();
            _files = files;
            _file = _files.Get<Dictionary<Setting, ISettingValue>>(new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.Settings), true);

            this.UpdateValues(_file.Value);
        }

        public SettingValue<T> Get<T>(Setting<T> setting) where T : notnull
        {
            ref ISettingValue? value = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, setting, out bool exists);

            if (exists && value is SettingValue<T> casted)
            {
                return casted;
            }

            value = casted = new SettingValue<T>(setting);
            this.Save();

            return casted;
        }

        public void Set<T>(Setting<T> setting, T value) where T : notnull
        {
            this.Get(setting).Value = value;
            this.Save();
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
            foreach (var (setting, value) in values)
            {
                ref ISettingValue? settingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, setting, out bool exists);

                if (exists == false)
                {
                    Type settingValueType = typeof(SettingValue<>).MakeGenericType(setting.Type);
                    settingValue = (ISettingValue)Activator.CreateInstance(settingValueType, setting)!;
                }

                settingValue!.Value = value.Value;
            }
        }
    }
}
