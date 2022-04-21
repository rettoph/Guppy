using Guppy.EntityComponent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface ITerminalService
    {
        internal void Update(GameTime gameTime);
        internal void Draw(GameTime gameTime);
    }
}
