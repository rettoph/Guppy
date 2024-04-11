using Guppy.Files;
using Guppy.Files.Services;
using Guppy.Resources.Constants;
using Guppy.Resources.Utilities;

namespace Guppy.Resources.Services
{
    internal sealed class SettingService : ISettingService, IDisposable
    {
        private readonly IFileService _files;
        private readonly IFile<IEnumerable<ISetting>> _file;

        public SettingService(IFileService files)
        {
            _files = files;
            _file = _files.Get<IEnumerable<ISetting>>(new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.Settings), true);
        }

        public void Save()
        {
            _file.Value = StaticCollection<ISetting>.GetAll();
            _files.Save(_file);
        }

        public void Dispose()
        {
            this.Save();

            StaticCollection<ISetting>.Clear();
        }
    }
}
