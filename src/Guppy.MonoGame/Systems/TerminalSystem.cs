using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Systems
{
    [GlobalScopeFilter]
    internal sealed class TerminalSystem : IUpdateSystem, IDrawSystem
    {
        private IFiltered<ITerminalService> _terminal;

        public TerminalSystem(IFiltered<ITerminalService> terminal)
        {
            _terminal = terminal;
        }

        public void Initialize(World world)
        {
            // throw new NotImplementedException();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            _terminal.Instance.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _terminal.Instance.Draw(gameTime);
        }
    }
}
