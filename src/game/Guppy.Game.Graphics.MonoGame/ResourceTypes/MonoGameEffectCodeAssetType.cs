using Guppy.Core.Files.Common;
using Guppy.Core.Files.Common.Helpers;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Game.Graphics.Common.Assets;
using Guppy.Game.Graphics.MonoGame.Assets;
using Guppy.Core.Logging.Common;

namespace Guppy.Game.MonoGame.AssetTypes
{
    internal class MonoGameEffectCodeAssetType(ILogger logger) : SimpleAssetType<IEffectCode>
    {
        private readonly ILogger _logger = logger;

        public override string Name => "EffectCode";

        protected override bool TryResolve(AssetKey<IEffectCode> resource, DirectoryPath root, string input, out IEffectCode value)
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
                this._logger.Error(ex, "Exception loading {Asset} at {Path}", resource.Name, path);
                value = null!;
                return false;
            }
        }
    }
}