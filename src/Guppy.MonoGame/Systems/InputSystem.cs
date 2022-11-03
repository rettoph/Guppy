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
    internal sealed class InputSystem : IUpdateSystem
    {
        private IInputService _inputs;

        public InputSystem(IInputService terminal)
        {
            _inputs = terminal;
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
            _inputs.Update(gameTime);
        }
    }
}
