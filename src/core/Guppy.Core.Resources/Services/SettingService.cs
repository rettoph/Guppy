using Guppy.Core.Common.Services;
using Guppy.Core.Common.Utilities;
using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Services;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Constants;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Core.Resources.Services
{
    internal sealed class SettingService : IHostedService, ISettingService, IDisposable
    {
        private bool _initialized;
        private readonly IFileService _files;
        private IFile<IEnumerable<ISetting>> _file;

        public SettingService(IFileService files)
        {
            _files = files;
            _file = null!;
        }

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

            _file = _files.Get<IEnumerable<ISetting>>(new FileLocation(DirectoryLocation.AppData(string.Empty), FilePaths.Settings), true);

            _initialized = true;
        }

        public void Save()
        {
            _file.Value = StaticCollection<ISetting>.GetAll();
            _files.Save(_file);
        }

        public void Dispose()
        {
            this.Save();

            StaticCollection<ISetting>.Clear(true);
        }
    }
}
