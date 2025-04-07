using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.ImGui.Common;
using Guppy.Core.Logging.Common;

namespace Guppy.Game.ImGui.MonoGame.ResourceTypes
{
    public class TrueTypeFontResourceType(ILogger logger) : SimpleResourceType<TrueTypeFont>
    {
        private readonly ILogger _logger = logger;

        protected override bool TryResolve(ResourceKey<TrueTypeFont> resource, DirectoryPath root, string input, out TrueTypeFont value)
        {
            string path = Path.Combine(root.Path, input);

            try
            {
                byte[] bytes;

                using (var stream = File.Open(path, FileMode.Open))
                {
                    using var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                value = new TrueTypeFont(bytes);
                return true;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, "Exception loading {Resource} at {Path}", resource.Name, path);
                value = null!;
                return false;
            }
        }
    }
}