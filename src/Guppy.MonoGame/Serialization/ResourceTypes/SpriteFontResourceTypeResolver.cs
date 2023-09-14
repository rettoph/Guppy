﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.Resources;
using Guppy.Resources.Serialization.Resources;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Serialization.Resources
{
    [AutoLoad]
    internal class SpriteFontResourceTypeResolver : ResourceTypeResolver<SpriteFont>
    {
        public Type Type => typeof(SpriteFont);

        private readonly ContentManager _content;

        public SpriteFontResourceTypeResolver(ContentManager content)
        {
            _content = content;
        }

        protected override void Configure(ResourcePack pack)
        {
            base.Configure(pack);

            _content.RootDirectory = pack.RootDirectory;
        }
        protected override bool TryResolve(Resource<SpriteFont> resource, string input, out SpriteFont value)
        {
            try
            {
                value = _content.Load<SpriteFont>(input);
                return true;
            }
            catch
            {
                value = default!;
                return false;
            }
            
        }
    }
}
