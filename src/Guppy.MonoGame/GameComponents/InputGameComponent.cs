using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.GameComponents
{
    [GlobalScopeFilter]
    internal sealed class InputGameComponent : SimpleGameComponent
    {
        private IInputService _inputs;

        public InputGameComponent(IInputService inputs)
        {
            _inputs = inputs;
        }

        public override void Update(GameTime gameTime)
        {
            _inputs.Update(gameTime);
        }
    }
}
