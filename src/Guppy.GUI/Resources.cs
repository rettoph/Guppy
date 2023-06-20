using Guppy.Resources;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public static class Resources
    {
        public static class Fonts
        {
            public static readonly Resource<SpriteFont> Default = new($"{nameof(Guppy)}.{nameof(Fonts)}.{nameof(Default)}");
            public static readonly Resource<SpriteFont> Debug = new($"{nameof(Guppy)}.{nameof(Fonts)}.{nameof(Debug)}");
        }
    }
}
