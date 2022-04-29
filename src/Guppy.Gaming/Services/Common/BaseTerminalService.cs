using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services.Common
{
    public abstract class BaseTerminalService : ITerminalService
    {
        public abstract void WriteLine(string text, Microsoft.Xna.Framework.Color color);

        protected abstract void Draw(GameTime gameTime);
        protected abstract void Update(GameTime gameTime);

        void ITerminalService.Draw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        void ITerminalService.Update(GameTime gameTime)
        {
            this.Update(gameTime);
        }
    }
}
