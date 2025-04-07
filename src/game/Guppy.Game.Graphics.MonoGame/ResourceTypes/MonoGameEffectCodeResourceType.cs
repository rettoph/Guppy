using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Game.Graphics.Common.Resources;
using Guppy.Game.Graphics.MonoGame.Resources;
using Guppy.Core.Logging.Common;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    internal class MonoGameEffectCodeResourceType(ILogger logger) : SimpleResourceType<IEffectCode>
    {
        private readonly ILogger _logger = logger;

        public override string Name => "EffectCode";

        protected override bool TryResolve(ResourceKey<IEffectCode> resource, DirectoryPath root, string input, out IEffectCode value)
        {
            string path = DirectoryHelper.Combine(root.Path, input);

            try
            {
                byte[] bytes;

                using (var stream = File.Open(path, FileMode.Open))
                {
                    using var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                value = new MonoGameEffectCode(bytes);
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