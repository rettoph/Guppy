using Guppy.MonoGame.UI.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public interface IElement
    {
        IStyleProvider Styles { get; }
        ElementState State { get; }

        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);

        void Clean(IStyleStack styles);
    }
}
