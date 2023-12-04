using Guppy.Attributes;
using Guppy.Common;
using Guppy.Files.Helpers;
using Guppy.Resources;
using Guppy.Resources.ResourceTypes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.MonoGame.ResourceTypes
{
    [AutoLoad]
    internal class SpriteFontResourceType : SimpleResourceType<SpriteFont>
    {
        public Type Type => typeof(SpriteFont);

        private readonly ContentManager _content;

        public SpriteFontResourceType(ContentManager content)
        {
            _content = content;
        }

        protected override bool TryResolve(Resource<SpriteFont> resource, string root, string input, out SpriteFont value)
        {
            _content.RootDirectory = root;

            value = _content.Load<SpriteFont>(DirectoryHelper.Normalize(input));
            return true;
        }
    }
}
