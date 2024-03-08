using Guppy.Files;
using Guppy.Files.Enums;
using Guppy.Files.Services;
using Guppy.Resources.Configuration;
using Guppy.Resources.Constants;

namespace Guppy.Resources.Extensions
{
    internal static class IFileServiceExtensions
    {
        public static IFile<ResourcePacksConfiguration> GetResourcePacksConfiguration(this IFileService files)
        {
            return files.Get<ResourcePacksConfiguration>(FileType.AppData, FilePaths.ResourcePacksConfiguration);
        }
    }
}
