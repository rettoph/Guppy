using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Resources
{
    public class SpriteFontResource : ContentResource<SpriteFont>
    {
        public SpriteFontResource(string name, string file) : base(name, file)
        {
        }

        public override void Export(string path, IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}
