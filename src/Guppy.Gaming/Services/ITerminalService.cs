using Guppy.EntityComponent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Services
{
    public interface ITerminalService
    {
        public void WriteLine(string text, XnaColor color);
        internal void Update(GameTime gameTime);
        internal void Draw(GameTime gameTime);
    }
}
