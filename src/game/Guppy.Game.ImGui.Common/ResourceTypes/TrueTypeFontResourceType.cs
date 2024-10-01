using Guppy.Core.Common.Attributes;
using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Serilog;

namespace Guppy.Game.ImGui.Common.ResourceTypes
{
    [AutoLoad]
    internal class TrueTypeFontResourceType(ILogger logger) : SimpleResourceType<TrueTypeFont>
    {
        private readonly ILogger _logger = logger;

        protected override bool TryResolve(Resource<TrueTypeFont> resource, DirectoryLocation root, string input, out TrueTypeFont value)
        {
            string path = Path.Combine(root.Path, input);

            try
            {
                byte[] bytes;

                using (var stream = File.Open(path, FileMode.Open))
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                }

                value = new TrueTypeFont(bytes);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{ClassName}::{MethodName} - Exception loading {Resource} at {Path}", nameof(TrueTypeFontResourceType), nameof(TryResolve), resource.Name, path);
                value = null!;
                return false;
            }
        }
    }
}
