using Guppy.Attributes;
using Guppy.Attributes;
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
    internal sealed class DebuggerSystem : IUpdateSystem, IDrawSystem
    {
        private IDebuggerService _debugger;

        public DebuggerSystem(IDebuggerService terminal)
        {
            _debugger = terminal;
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
            _debugger.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _debugger.Draw(gameTime);
        }
    }
}
