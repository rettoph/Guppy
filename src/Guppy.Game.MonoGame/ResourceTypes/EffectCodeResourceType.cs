﻿using Guppy.Attributes;
using Guppy.Files;
using Guppy.Files.Helpers;
using Guppy.Game.MonoGame.Graphics.Effects;
using Guppy.Resources;
using Guppy.Resources.ResourceTypes;
using Serilog;

namespace Guppy.Game.MonoGame.ResourceTypes
{
    [AutoLoad]
    internal class EffectCodeResourceType : SimpleResourceType<EffectCode>
    {
        private readonly ILogger _logger;

        public EffectCodeResourceType(ILogger logger)
        {
            _logger = logger;
        }

        protected override bool TryResolve(Resource<EffectCode> resource, DirectoryLocation root, string input, out EffectCode value)
        {
            string path = DirectoryHelper.Combine(root.Path, input);

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
            catch (Exception ex)
            {
                _logger.Error(ex, "{ClassName}::{MethodName} - Exception loading {Resource} at {Path}", nameof(EffectCodeResourceType), nameof(TryResolve), resource.Name, path);
                value = null!;
                return false;
            }
        }
    }
}
