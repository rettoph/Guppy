using Guppy.Attributes;
using Guppy.MonoGame.Graphics.Effects;
using Guppy.Resources;
using Guppy.Resources.ResourceTypes;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace Guppy.MonoGame.ResourceTypes
{
    [AutoLoad]
    internal class EffectCodeResourceType : ResourceType<EffectCode>
    {
        private readonly ILogger _logger;

        public EffectCodeResourceType(ILogger logger)
        {
            _logger = logger;
        }

        protected override bool TryResolve(Resource<EffectCode> resource, string input, string root, out EffectCode value)
        {
            string path = Path.Combine(root, input);

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

                value = new EffectCode(bytes);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "{ClassName}::{MethodName} - Exception loading {Resource} at {Path}", nameof(EffectCodeResourceType), nameof(TryResolve), resource.Name, path);
                value = null!;
                return false;
            }
        }
    }
}
