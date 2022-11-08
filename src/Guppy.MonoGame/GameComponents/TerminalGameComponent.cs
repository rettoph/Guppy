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
    internal sealed class TerminalGameComponent : SimpleDrawableGameComponent
    {
        private readonly IFiltered<ITerminalService> _terminal;

        public TerminalGameComponent(IFiltered<ITerminalService> terminal)
        {
            _terminal = terminal;
        }

        public override void Update(GameTime gameTime)
        {
            _terminal.Instance!.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _terminal.Instance!.Draw(gameTime);
        }
    }
}
